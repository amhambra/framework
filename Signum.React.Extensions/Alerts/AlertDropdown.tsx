import * as React from 'react'
import * as Operations from '@framework/Operations'
import * as Finder from '@framework/Finder'
import { Entity, getToString, is, JavascriptMessage, liteKey, parseLite, toLite } from '@framework/Signum.Entities'
import { Toast, Button, ButtonGroup } from 'react-bootstrap'
import { DateTime } from 'luxon'
import { useAPIWithReload, useForceUpdate, useThrottle, useUpdatedRef } from '@framework/Hooks';
import * as AuthClient from '../Authorization/AuthClient'
import * as Navigator from '@framework/Navigator'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { AlertDropDownGroup, AlertEntity, AlertMessage, AlertOperation } from './Signum.Entities.Alerts'
import * as AlertsClient from './AlertsClient'
import "./AlertDropdown.css"
import { Link } from 'react-router-dom';
import { classes, Dic } from '@framework/Globals'
import { Lite } from '@framework/Signum.Entities'
import MessageModal from '@framework/Modals/MessageModal'
import { useSignalRCallback, useSignalRConnection, useSignalRGroup } from './useSignalR'
import { SmallProfilePhoto } from '../Authorization/Templates/ProfilePhoto'
import { UserEntity } from '../Authorization/Signum.Entities.Authorization'
import { translate } from '../Chart/D3Scripts/Components/ChartUtils'
import { useRootClose } from '@restart/ui'

const MaxNumberOfAlerts = 3;
const MaxNumberOfGroups = 3;

export default function AlertDropdown(props: { keepRingingFor?: number }) {

  if (!Navigator.isViewable(AlertEntity))
    return null;

  return <AlertDropdownImp keepRingingFor={props.keepRingingFor ?? 10 * 1000} />;
}

interface AlertGroupWithSize {
  groupTarget?: Lite<Entity>; 
  alerts: AlertWithSize[];
  totalHight?: number;
  maxDate: string;
}

interface AlertWithSize{
  alert: AlertEntity,
  height?: number;
}

function AlertDropdownImp(props: { keepRingingFor: number }) {

  const conn = useSignalRConnection("~/api/alertshub");

  useSignalRGroup(conn, {
    enterGroup: c => c.invoke("Login", AuthClient.getAuthToken()),
    exitGroup: c => c.send("Logout"),
    deps: [AuthClient.getAuthToken()]
  });

  useSignalRCallback(conn, "AlertsChanged", () => {
    reloadCount();
  }, []);

  const forceUpdate = useForceUpdate();
  const [isOpen, setIsOpen] = React.useState<boolean>(false);
  const [ringing, setRinging] = React.useState<boolean>(false);
  const ringingRef = useUpdatedRef(ringing);

  const [showGroups, setShowGroups] = React.useState<number>(MaxNumberOfGroups);
  
  const isOpenRef = useUpdatedRef(isOpen);

  const [alertGroups, setAlertsWithSize] = React.useState<AlertGroupWithSize[] | undefined>(undefined);

  var [countResult, reloadCount] = useAPIWithReload<AlertsClient.NumAlerts>((signal, oldResult) => AlertsClient.API.myAlertsCount().then(res => {
    if (res.lastAlert != null) {
      if (oldResult == null || oldResult.lastAlert == null || oldResult.lastAlert < res.lastAlert) {
        if (!ringingRef.current)
          setRinging(true);
      }

      if (isOpenRef.current) {
        AlertsClient.API.myAlerts()
          .then(als => {
            setAlerts(als, true);
          });
      }

    } else {
      if (ringingRef.current)
        setRinging(false);

      setAlertsWithSize([]);
    }

    return res;
  }), [], { avoidReset: true });

  React.useEffect(() => {
    if (ringing) {
      var handler = setTimeout(() => {
        setRinging(false);
      }, props.keepRingingFor);

      return () => { clearTimeout(handler) };
    }
  }, [ringing]);


  function setAlerts(alerts: AlertEntity[], keepGroupOrder: boolean) {
    var dictionary = alertGroups?.flatMap(a => a.alerts).notNull().toObject(a => liteKey(toLite(a.alert)), a => a.height);
    var newGroup = alerts.orderByDescending(a => a.alertDate).groupBy(a => a.groupTarget ? liteKey(a.groupTarget) : "null");

    if (keepGroupOrder && alertGroups?.length == newGroup.length && alertGroups.every(a =>
      newGroup.some(n => a.groupTarget != undefined && liteKey(a.groupTarget) == n.key || a.groupTarget == undefined && n.key == "null"))) {
      var oldGroup = alertGroups.clone();
      oldGroup.forEach(g => {
        g.alerts.clear;
        g.alerts = newGroup.filter(n => n.key == (g.groupTarget ? liteKey(g.groupTarget) : "null")).first().elements.map<AlertWithSize>(a => ({ alert: a, height: dictionary?.[liteKey(toLite(a))] }));
      });
      setAlertsWithSize(oldGroup);
    }
    else {
      setAlertsWithSize(alerts.orderByDescending(a => a.alertDate).groupBy(a => a.groupTarget ? liteKey(a.groupTarget) : "null").map(gr => (
        {
          groupTarget: gr.key != "null" ? parseLite(gr.key) : undefined,
          alerts: gr.elements.map<AlertWithSize>(a => ({ alert: a, height: dictionary?.[liteKey(toLite(a))] })),
          maxDate: gr.elements.orderByDescending(a => a.alertDate!).first().alertDate!,
          totalHight: gr.elements.sum(a => dictionary?.[liteKey(toLite(a))] ?? 0)
        })));
    }
  }

  function handleOnToggle() {

    if (!isOpen) {
      AlertsClient.API.myAlerts()
        .then(alerts => setAlerts(alerts, false));
    }

    setIsOpen(!isOpen);
  }

  function handleOnCloseAlerts(toRemove: AlertWithSize[]) {
    //Optimistic
    let wasClosed = false;
    if (alertGroups) {
      if (toRemove.length > 1)
        alertGroups.extract(ag => ag.alerts.some(a => is(a.alert, toRemove[0].alert)));
      else
        alertGroups.filter(ag => ag.alerts.some(a => is(a.alert, toRemove[0].alert))).first().alerts.extract(a => is(a.alert, toRemove[0].alert));

      if (alertGroups.length == 0) {
        setIsOpen(false);
        wasClosed = true;
      }
    }
    if (countResult)
      countResult.numAlerts -= toRemove.length;
    forceUpdate();

    Operations.API.executeMultiple(toRemove.map(a => toLite(a.alert)), AlertOperation.Attend)
      .then(res => {

        const errors = Dic.getValues(res.errors).filter(a => Boolean(a));
        if (errors.length) {
          MessageModal.showError(<ul>{errors.map((a, i) => <li key={i}>{a}</li>)}</ul>, "Errors attending alerts");
        }

        // Pesimistic
        AlertsClient.API.myAlerts()
          .then(alerts => {
            if (wasClosed && alerts.length > 0)
              setIsOpen(true);

            setAlerts(alerts, true);
          });

        reloadCount();

      });
  }

  var divRef = React.useRef<HTMLDivElement>(null);

  useRootClose(divRef, () => setIsOpen(false), { disabled: !isOpen });

  return (
    <>
      <div className="nav-link sf-bell-container" onClick={handleOnToggle} title={window.__disableSignalR ?? undefined}>
        <FontAwesomeIcon icon={window.__disableSignalR ? "bell-slash" : "bell"} className={classes("sf-bell", ringing && "ringing", isOpen && "open", countResult && countResult.numAlerts > 0 && "active")} />
        {countResult && countResult.numAlerts > 0 && <span className="badge btn-danger badge-pill sf-alerts-badge">{countResult.numAlerts}</span>}
      </div>
      {isOpen && <div className="sf-alerts-toasts mt-2" style={{
        backgroundColor : "rgba(255,255,255, 0.7)",
      }}>
        {alertGroups == null ? <Toast> <Toast.Body>{JavascriptMessage.loading.niceToString()}</Toast.Body></Toast> :

          <>
            {alertGroups.length == 0 && <Toast><Toast.Body>{AlertMessage.YouDoNotHaveAnyActiveAlert.niceToString()}</Toast.Body></Toast>}

            <div style={{ position: 'relative' }}>
              {alertGroups.orderByDescending(a => a.maxDate).filter((gr, i) => i < showGroups).flatMap((gr, i) => [<AlertGroupToast

                key={gr.groupTarget?.id ?? "null"}
                group={gr}
                onClose={handleOnCloseAlerts}
                refresh={reloadCount}
                onSizeSet={forceUpdate}
                style={{
                  width: "100%",
                  position: 'absolute',
                  transform: `translateY(${alertGroups.orderByDescending(a => a.maxDate).filter((a, j) => j < i).sum(a => (a.totalHight ?? 0))}px)`,
                  //transform: `translateY(${i * 200 + 100}px)`,
                  transition: "transform 0.4s ease"
                }}
              />])
              }
            </div>
            {showGroups < alertGroups.length && <div style={{
              transform: `translateY(${alertGroups.orderByDescending(a => a.maxDate).filter((a, j) => j < showGroups).sum(a => (a.totalHight ?? 0))}px)`,
              transition: "transform 0.4s ease"
            }} >
              <Toast className="w-100 my-2">
                <Toast.Body style={{ textAlign: "center" }}>
                  <span onClick={() => setShowGroups(showGroups + MaxNumberOfGroups)} style={{ cursor: 'pointer', color: 'darkgray', fontSize: "0.8rem", fontWeight: 'bold' }}>
                    {AlertMessage.Show0GroupsMore1Remaining.niceToString(MaxNumberOfGroups, alertGroups.length)}
                  </span>
                </Toast.Body>
              </Toast>
            </div>}

            <div style={{
              transform: `translateY(${alertGroups.orderByDescending(a => a.maxDate).filter((a, j) => j < showGroups).sum(a => (a.totalHight ?? 0))}px)`,
              transition: "transform 0.4s ease"
            }} > 
              <Toast className="w-100 mt-2">
                <Toast.Body style={{ textAlign: "center" }}>
                  <Link onClick={() => setIsOpen(false)} to={Finder.findOptionsPath({
                    queryName: AlertEntity,
                    filterOptions: [
                      { token: AlertEntity.token(a => a.entity.recipient), value: AuthClient.currentUser() },
                    ],
                    orderOptions: [
                      { token: AlertEntity.token(a => a.entity.alertDate), orderType: "Descending" },
                    ],
                    columnOptions: [
                      { token: AlertEntity.token(a => a.entity.id) },
                      { token: AlertEntity.token(a => a.entity.alertDate) },
                      { token: AlertEntity.token(a => a.entity.alertType) },
                      { token: AlertEntity.token("Text") },
                      { token: AlertEntity.token(a => a.entity.target) },
                      { token: AlertEntity.token(a => a.entity).expression("CurrentState") },
                      { token: AlertEntity.token(a => a.entity.createdBy) },
                      { token: AlertEntity.token(a => a.entity.recipient) },
                    ],
                    columnOptionsMode: "ReplaceAll"
                  })}>{AlertMessage.AllMyAlerts.niceToString()}</Link>
                </Toast.Body>
              </Toast>
            </div>
          </>
        }
      </div>}
    </>
  );
}



export function AlertGroupToast(p: { group: AlertGroupWithSize, onClose: (e: AlertWithSize[]) => void, refresh: () => void, style?: React.CSSProperties | undefined , onSizeSet: () => void}) {

  const [showAlerts, setShowAlert] = React.useState<number>(1);
  const [showHiddenAlerts, setShowHiddenAlert] = React.useState<number>(MaxNumberOfAlerts);

  const showAlertsAndHidden = showAlerts + showHiddenAlerts;
  var alerts = p.group.alerts.filter((a, i) => i < showAlertsAndHidden);

  const forceUpdate = useForceUpdate();

  const groupTarget = p.group.alerts[0]?.alert.groupTarget;

  var htmlRef = React.useRef<HTMLDivElement>(null);

  React.useEffect(() => {
    p.group.totalHight = htmlRef.current?.getBoundingClientRect().height;
    p.onSizeSet();
  }, [p.group, p.refresh]);

  const lastExpandedAlert = alerts.filter((a, i) => i < showAlerts)?.lastOrNull();

  const totalExpandedHeight = alerts.filter((a, i) => i < showAlerts).sum((a, i) => (a.height ?? 0));

  const textStyle: React.CSSProperties = { color: 'darkgray', fontSize: "0.8rem", fontWeight: 'bold' };
  return (
    <div className="sf-alert-group pb-2" style={p.style} ref={htmlRef}>
      <div className="p-2 mt-2 d-flex" style={{ backdropFilter: "blur(10px)", position: 'relative' }}>
        {groupTarget ? <span style={textStyle}>{`${getToString(groupTarget)} (${p.group.alerts.length})`}</span> : <span style={textStyle} >{`${AlertMessage.OtherNotifications.niceToString()} (${p.group.alerts.length})`}</span>}

        {alerts.length > 1 && <span className="ms-auto me-2" style={{ cursor: 'pointer', ...textStyle }} onClick={() => setShowAlert(showAlerts == 1 ? 1 + MaxNumberOfAlerts : 1)}>{showAlerts == 1 ? AlertMessage.Expand.niceToString() : AlertMessage.Collapse.niceToString()}</span>}

        {alerts.length > 1 && <span style={{ whiteSpace: 'nowrap', cursor: 'pointer', ...textStyle }} onClick={() => p.onClose(p.group.alerts)}>{AlertMessage.CloseAll.niceToString()}</span>}
      </div>
      <div style={{
        perspective: "1000px",
        position: 'relative',
        marginBottom: (Math.max(0, (alerts.length - showAlerts)) * 8) + "px",
        height: alerts?.filter((a, i) => i < showAlerts).sum(a => (a.height ?? 0) + 2),
        transition: "transform .4s ease",
      }}>
        {alerts.map((a, i) => {
          var expanded = i < showAlerts;

          var hiddenIndex = (i - (showAlerts - 1));

          return (
            <AlertToast key={a.alert.id} alert={a} onClose={p.onClose}
              expanded={expanded}
              onSizeSet={forceUpdate}
              refresh={p.refresh} className="mb-0 mt-0"
              style={{
                borderRadius: ".15em",
                boxShadow: "0 0 2px 1px rgba(0, 0, 0, 0.1), 0 2px 3px rgba(0, 0, 0, 0.16)",
                width: "100%",
                transformOrigin: "50% 0",
                position: "absolute",
                zIndex: -i,
                maxHeight: expanded ? undefined : lastExpandedAlert?.height,
                overflow: expanded ? undefined : 'hidden',
                transform: expanded ? `translateY(${alerts.filter((alert, j) => j < i).sum(alert => (alert?.height ?? 0) + 2)}px)` :
                  `translate3d(0, ${totalExpandedHeight - (a.height ?? 0) + hiddenIndex * 8}px,                      ${-hiddenIndex * 32}px)`,
                opacity: expanded ? undefined : Math.max(0, 1 - (hiddenIndex * 0.2)),
                transition: "transform .4s ease",
              }} />
          );
        })}
      </div>
      {showAlerts < p.group.alerts.length && showAlerts > 1 && <div style={{ position: 'relative', backdropFilter: "blur(10px)", textAlign: 'center', marginTop: "-10px" }}>
        <span onClick={() => setShowAlert(showAlerts + MaxNumberOfAlerts)} style={{ cursor: 'pointer', color: 'darkgray', fontSize: "0.8rem", fontWeight: 'bold' }}>
          {AlertMessage.Show0AlertsMore.niceToString(MaxNumberOfAlerts)}
        </span>
      </div>}
    </div>
  );
}

export function AlertToast(p: { alert: AlertWithSize, onSizeSet: () => void, expanded: boolean, onClose: (e: AlertWithSize[]) => void, refresh: () => void, className?: string, style?: React.CSSProperties | undefined }) {

  var alert = p.alert.alert;

  var icon = alert.alertType && alert.alertType.key && AlertToast.icons[alert.alertType.key];

  var wasExpanded = useThrottle(p.expanded, 0.4 * 1000);

  var htmlRef = React.useRef<HTMLDivElement>(null);

  React.useEffect(() => {
    p.alert.height = htmlRef.current?.getBoundingClientRect().height;
    p.onSizeSet();
  }, [p.alert, p.expanded, wasExpanded]);

  return (
    <Toast ref={htmlRef} onClose={() => p.onClose([p.alert])} className={classes(p.className, "w-100")} style={p.style}>
      <Toast.Header>
        {icon && <span className="me-2">{icon}</span>}
        <strong className="me-auto">{AlertsClient.getTitle(alert.titleField, alert.alertType)}</strong>
        <small>{DateTime.fromISO(alert.alertDate!).toRelative()}</small>
      </Toast.Header>
      <Toast.Body style={{ whiteSpace: "pre-wrap", opacity: p.expanded ? undefined : 0, transition: "transform .4s ease", }}>
        <div className="row">
          <div className="col-sm-1">
            {alert.createdBy && <SmallProfilePhoto user={alert.createdBy as Lite<UserEntity>} />}
          </div>
          <div className="col-sm-11" style={{ overflow: 'hidden', textOverflow: 'ellipsis' }}>
            {AlertsClient.formatText(alert.textField || alert.textFromAlertType || "", alert, p.refresh)}
          </div>
        </div>
      </Toast.Body>
    </Toast>
  );
}

AlertToast.icons = {} as { [alertTypeKey: string]: React.ReactNode };
