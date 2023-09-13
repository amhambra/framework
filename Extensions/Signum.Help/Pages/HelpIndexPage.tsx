import * as React from 'react'
import { useLocation, useParams, Link } from 'react-router-dom'
import * as Navigator from '@framework/Navigator'
import EntityLink from '@framework/SearchControl/EntityLink'
import { API, Urls } from '../HelpClient'
import { SearchControl } from '@framework/Search';
import { useAPI } from '@framework/Hooks';
import { HelpMessage, NamespaceHelpEntity, AppendixHelpEntity } from '../Signum.Help';
import { getTypeInfo } from '@framework/Reflection';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTitle } from '@framework/AppContext'


export default function HelpIndexPage() {

  useTitle(HelpMessage.Help.niceToString());

  var index = useAPI(() => API.index(), []);

  return (
    <div id="entityContent">
      <h1 className="display-5">{HelpMessage.Help.niceToString()}</h1>

      {index && <div>
        {index.namespaces.groupBy(a => a.module ?? "none").orderBy(a => a.key == "Signum")
          .map(gr => <div key={gr.key} className="my-4 ms-4">
            <h2>{gr.key}</h2>
            <ul className="responsive-columns">
              {gr.elements.orderBy(a => a.namespace).map(nh =>
                <li className="mb-4" key={nh.namespace}>
                  <h4 className="display-7">
                    <Link to={Urls.namespaceUrl(nh.namespace)}>{nh.title}</Link>
                    {nh.namespace.after(".").tryBeforeLast(".") && <small> {HelpMessage.In0.niceToString(nh.namespace.after(".").tryBeforeLast("."))}</small>}
                  </h4>
                  <ul>
                    {nh.allowedTypes.map(ei => <li key={ei.cleanName}><Link to={Urls.typeUrl(ei.cleanName)} >{getTypeInfo(ei.cleanName).niceName}</Link></li>)}
                  </ul>
                </li>
              )}
            </ul>
          </div>
          )}

        <h3 className="display-5">
          {HelpMessage.Appendices.niceToString()}
          {Navigator.isCreable(AppendixHelpEntity, { customComponent: true, isSearch: true }) && <Link to={Urls.appendixUrl(null)} style={{ fontSize: "20px" }} ><FontAwesomeIcon icon="plus" className="ms-2" title={HelpMessage.Appendices.niceToString()} /></Link>}
        </h3>
        <ul className="responsive-columns">
          {index.appendices.map(ap => <li key={ap.uniqueName}><Link to={Urls.appendixUrl(ap.uniqueName)} >{ap.title}</Link></li>)}
        </ul>
      </div>
      }
    </div>
  );
}



