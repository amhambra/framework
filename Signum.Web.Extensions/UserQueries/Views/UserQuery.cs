﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
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
    using Signum.Engine;
    using Signum.Entities.Reports;
    using Signum.Entities.Basics;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MvcRazorClassGenerator", "1.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/UserQueries/Views/UserQuery.cshtml")]
    public class _Page_UserQueries_Views_UserQuery_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
#line hidden

        public _Page_UserQueries_Views_UserQuery_cshtml()
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



WriteLiteral("\r\n");


 using (var e = Html.TypeContext<UserQueryDN>())
{
    
Write(Html.EntityLine(e, f => f.Related, el => el.Create = false));

                                                                
using (var query = e.SubContext(f => f.Query))
{
    
Write(Html.HiddenRuntimeInfo(query));

                                  
    
Write(Html.Span("Query", "Query", "sf-label-line"));

                                             
    
Write(Html.Href("hrefQuery", query.Value.DisplayName, Navigator.FindRoute(ViewData[ViewDataKeys.QueryName]), "", "sf-value-line", null));

                                                                                                                                  
        

WriteLiteral("    <div class=\"clearall\">\r\n    </div>\r\n");


        
    
Write(Html.Hidden(query.Compose("Key"), query.Value.Key));

                                                       
    
Write(Html.Hidden(query.Compose("DisplayName"), query.Value.DisplayName));

                                                                       
}
    
Write(Html.ValueLine(e, f => f.DisplayName));

                                          

WriteLiteral("    <br />\r\n");


    
Write(Html.EntityRepeater(e, f => f.Filters, er => { er.PreserveViewData = true; er.ForceNewInUI = true; }));

                                                                                                          

WriteLiteral("    <br />\r\n");


    
Write(Html.ValueLine(e, f => f.ColumnsMode));

                                          
    
Write(Html.EntityRepeater(e, f => f.Columns, er => { er.PreserveViewData = true; er.ForceNewInUI = true; }));

                                                                                                          

WriteLiteral("    <br />\r\n");


    
Write(Html.EntityRepeater(e, f => f.Orders, er => { er.PreserveViewData = true; er.ForceNewInUI = true; }));

                                                                                                         

WriteLiteral("    <br />\r\n");


    
Write(Html.ValueLine(e, f => f.MaxItems));

                                       
}
WriteLiteral(" ");


        }
    }
}
