﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Signum.Entities;
    
    #line 1 "..\..\Basic\Views\DateSpan.cshtml"
    using Signum.Entities.Basics;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Basic/Views/DateSpan.cshtml")]
    public partial class _Basic_Views_DateSpan_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Basic_Views_DateSpan_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 3 "..\..\Basic\Views\DateSpan.cshtml"
 using (var s = Html.TypeContext<DateSpanEntity>())
{

            
            #line default
            #line hidden
WriteLiteral("    <table");

WriteLiteral(" id=\'datespan\'");

WriteLiteral(">\r\n        <tr>\r\n            <td>\r\n");

WriteLiteral("                ");

            
            #line 8 "..\..\Basic\Views\DateSpan.cshtml"
           Write(Html.ValueLine(s, e => e.Years, vl => vl.ValueHtmlProps.Add("size", 3)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n            <td>\r\n");

WriteLiteral("                ");

            
            #line 11 "..\..\Basic\Views\DateSpan.cshtml"
           Write(Html.ValueLine(s, e => e.Months, vl => vl.ValueHtmlProps.Add("size", 3)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n            <td>\r\n");

WriteLiteral("                ");

            
            #line 14 "..\..\Basic\Views\DateSpan.cshtml"
           Write(Html.ValueLine(s, e => e.Days, vl => vl.ValueHtmlProps.Add("size", 3)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </td>\r\n        </tr>\r\n    </table> \r\n");

            
            #line 18 "..\..\Basic\Views\DateSpan.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
