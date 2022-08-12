import * as React from 'react'
import { DateTime } from 'luxon'
import { EntityCheckboxList, ValueLine } from '@framework/Lines'
import { TypeContext } from '@framework/TypeContext'
import { AlertEntity, AlertState, AlertTypeSymbol, SendNotificationEmailTaskEntity } from '../Signum.Entities.Alerts'
import { useForceUpdate } from '../../../Signum.React/Scripts/Hooks';
import { SearchValueLine } from '../../../Signum.React/Scripts/Search';
import { toLite } from '@framework/Signum.Entities'

export default function SendNotificationEmailTask(p: { ctx: TypeContext<SendNotificationEmailTaskEntity> }) {
  const ctx = p.ctx;
  const forceUpdate = useForceUpdate();

  return (
    <div>
      <ValueLine ctx={ctx.subCtx(n => n.sendNotificationsOlderThan)} labelColumns={4} onChange={forceUpdate} valueColumns={2} />
      <ValueLine ctx={ctx.subCtx(n => n.ignoreNotificationsOlderThan)} labelColumns={4} onChange={forceUpdate} valueColumns={2}/>
      <ValueLine ctx={ctx.subCtx(n => n.sendBehavior)} labelColumns={4} onChange={forceUpdate} />
      {(ctx.value.sendBehavior == "Exclude" || ctx.value.sendBehavior == "Include") && < EntityCheckboxList ctx={ctx.subCtx(n => n.alertTypes)} columnCount={1} onChange={forceUpdate}/>}
      <SearchValueLine ctx={ctx} findOptions={{
        queryName: AlertEntity,
        filterOptions: [
          { token: AlertEntity.token(a => a.entity.state), value: AlertState.value("Saved") },
          { token: AlertEntity.token(a => a.entity.emailNotificationsSent), value: false },
          { token: AlertEntity.token(a => a.entity.recipient), operation: "DistinctTo", value: null },
          ctx.value.sendBehavior == "All" ? null :
            {
              token: AlertEntity.token(a => a.entity.alertType),
              operation: ctx.value.sendBehavior == "Include" ? "IsIn" : "IsNotIn",
              value: ctx.value.alertTypes.map(at => toLite(at.element))
            },
          { token: AlertEntity.token(a => a.entity.alertDate), operation: "LessThan", value: DateTime.local().minus({ minutes: ctx.value.sendNotificationsOlderThan }).toISO() },
          ctx.value.ignoreNotificationsOlderThan == null ? null : {
            token: AlertEntity.token(a => a.entity.alertDate),
            operation: "GreaterThan",
            value: DateTime.local().minus({ days: ctx.value.ignoreNotificationsOlderThan! }).toISO()
          },
        ],
        groupResults: true,
        columnOptions: [
          { token: AlertEntity.token().count() },
          { token: AlertEntity.token(a => a.recipient) },
        ],
        columnOptionsMode: "ReplaceAll"
      }} />
    </div>
  );
}
