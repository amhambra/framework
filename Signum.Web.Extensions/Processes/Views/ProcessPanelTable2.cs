﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Signum.Utilities;
    using Signum.Entities;
    using Signum.Web;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Caching;
    using System.Web.DynamicData;
    using System.Web.SessionState;
    using System.Web.Profile;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    using System.Xml.Linq;
    using Signum.Utilities.ExpressionTrees;
    using Signum.Engine.Processes;
    using Signum.Web.Processes;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Processes/Views/ProcessPanelTable.cshtml")]
    public class _Page_Processes_Views_ProcessPanelTable_cshtml : System.Web.Mvc.WebViewPage<ProcessLogicState>
    {


        public _Page_Processes_Views_ProcessPanelTable_cshtml()
        {
        }
        protected System.Web.HttpApplication ApplicationInstance
        {
            get
            {
                return ((System.Web.HttpApplication)(Context.ApplicationInstance));
            }
        }
        public override void Execute()
        {




WriteLiteral("<div id=\"processMainDiv\">\r\n    State: ");


       Write(Model.Running ? "RUNNING" : "STOPPED");

WriteLiteral("\r\n    <br />\r\n    MaxDegreeOfParallelism: ");


                       Write(Model.MaxDegreeOfParallelism);

WriteLiteral("\r\n    <br />\r\n    InitialDelayMiliseconds: ");


                        Write(Model.InitialDelayMiliseconds);

WriteLiteral(@"
    <br />
    <table class=""sf-search-results sf-stats-table"">
        <thead>
            <tr>
                <th>
                    Process
                </th>
                <th>
                    State
                </th>
                <th>
                    IsCancellationRequested
                </th>
            </tr>
        </thead>
        <tbody>
");


             foreach (var item in Model.Queue)
            {

WriteLiteral("                <tr>\r\n                    <td>\r\n                        <a class=" +
"\"sf-stats-show\">Show</a>\r\n                    </td>\r\n                    <td>");


                   Write(Html.LightEntityLine(item.ProcessExecution, true));

WriteLiteral("\r\n                    </td>\r\n                    <td>");


                   Write(item.State);

WriteLiteral("\r\n                    </td>\r\n                    <td>");


                   Write(item.IsCancellationRequested);

WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");


            }

WriteLiteral("        </tbody>\r\n    </table>\r\n    }\r\n    <br />\r\n</div>\r\n");


        }
    }
}
