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
    
    #line 1 "..\..\Dashboard\Views\Admin\SearchControlPart.cshtml"
    using System.Configuration;
    
    #line default
    #line hidden
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
    
    #line 2 "..\..\Dashboard\Views\Admin\SearchControlPart.cshtml"
    using Signum.Entities.Dashboard;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Dashboard\Views\Admin\SearchControlPart.cshtml"
    using Signum.Entities.DynamicQuery;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    #line 3 "..\..\Dashboard\Views\Admin\SearchControlPart.cshtml"
    using Signum.Web.Dashboard;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Dashboard/Views/Admin/SearchControlPart.cshtml")]
    public partial class _Dashboard_Views_Admin_SearchControlPart_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Dashboard_Views_Admin_SearchControlPart_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 6 "..\..\Dashboard\Views\Admin\SearchControlPart.cshtml"
 using(var tc = Html.TypeContext<UserQueryPartEntity>())
{
    
            
            #line default
            #line hidden
            
            #line 8 "..\..\Dashboard\Views\Admin\SearchControlPart.cshtml"
Write(Html.EntityLine(tc, pp => pp.UserQuery, el => el.Create = false));

            
            #line default
            #line hidden
            
            #line 8 "..\..\Dashboard\Views\Admin\SearchControlPart.cshtml"
                                                                     
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
