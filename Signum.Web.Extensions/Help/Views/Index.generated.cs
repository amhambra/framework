﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Signum.Web.Extensions.Help.Views
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
    
    #line 4 "..\..\Help\Views\Index.cshtml"
    using Signum.Engine.Help;
    
    #line default
    #line hidden
    using Signum.Entities;
    
    #line 3 "..\..\Help\Views\Index.cshtml"
    using Signum.Entities.Help;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    #line 1 "..\..\Help\Views\Index.cshtml"
    using Signum.Web.Extensions;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Help\Views\Index.cshtml"
    using Signum.Web.Help;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Help/Views/Index.cshtml")]
    public partial class Index : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Index()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Help\Views\Index.cshtml"
    
    ViewBag.Title = HelpMessage.HelpDocumentation.NiceToString();

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

DefineSection("head", () => {

WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 11 "..\..\Help\Views\Index.cshtml"
Write(Html.ScriptCss("~/help/Content/help.css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

});

WriteLiteral("<div");

WriteLiteral(" id=\"entityContent\"");

WriteLiteral(">\r\n    <h1>");

            
            #line 14 "..\..\Help\Views\Index.cshtml"
   Write(HelpMessage.HelpDocumentation.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("</h1>\r\n    <table>\r\n        <tr>\r\n");

            
            #line 17 "..\..\Help\Views\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 17 "..\..\Help\Views\Index.cshtml"
             foreach (NamespaceModel item in ((NamespaceModel)Model).Namespaces)
            {

            
            #line default
            #line hidden
WriteLiteral("                <td>\r\n");

            
            #line 20 "..\..\Help\Views\Index.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 20 "..\..\Help\Views\Index.cshtml"
                       Html.RenderPartial(HelpClient.NamespaceControlUrl, item); 
            
            #line default
            #line hidden
WriteLiteral("\r\n                </td>\r\n");

            
            #line 22 "..\..\Help\Views\Index.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </tr>\r\n    </table>\r\n");

            
            #line 25 "..\..\Help\Views\Index.cshtml"
    
            
            #line default
            #line hidden
            
            #line 25 "..\..\Help\Views\Index.cshtml"
     if (ViewData.TryGetC("appendices") != null)
    {
        List<AppendixHelp> appendices = (List<AppendixHelp>)ViewData["appendices"];
        if (appendices.Count > 0)
        {

            
            #line default
            #line hidden
WriteLiteral("        <h2>");

            
            #line 30 "..\..\Help\Views\Index.cshtml"
       Write(HelpMessage.Appendices.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n");

WriteLiteral("        <ul>\r\n");

            
            #line 32 "..\..\Help\Views\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 32 "..\..\Help\Views\Index.cshtml"
             foreach (var a in appendices)
            {

            
            #line default
            #line hidden
WriteLiteral("                <li>\r\n");

WriteLiteral("                    ");

            
            #line 35 "..\..\Help\Views\Index.cshtml"
               Write(Html.ActionLink(a.Title, "ViewAppendix", new { appendix = a.Name }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </li>\r\n");

            
            #line 37 "..\..\Help\Views\Index.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </ul>\r\n");

            
            #line 39 "..\..\Help\Views\Index.cshtml"
        }
    }

            
            #line default
            #line hidden
WriteLiteral("</div>");

        }
    }
}
#pragma warning restore 1591
