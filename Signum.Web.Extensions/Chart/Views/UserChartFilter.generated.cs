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

namespace Signum.Web.Extensions.Chart.Views
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
    
    #line 1 "..\..\Chart\Views\UserChartFilter.cshtml"
    using Signum.Engine;
    
    #line default
    #line hidden
    using Signum.Entities;
    
    #line 5 "..\..\Chart\Views\UserChartFilter.cshtml"
    using Signum.Entities.Chart;
    
    #line default
    #line hidden
    
    #line 6 "..\..\Chart\Views\UserChartFilter.cshtml"
    using Signum.Entities.DynamicQuery;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Chart\Views\UserChartFilter.cshtml"
    using Signum.Entities.UserQueries;
    
    #line default
    #line hidden
    using Signum.Utilities;
    
    #line 3 "..\..\Chart\Views\UserChartFilter.cshtml"
    using Signum.Web;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Chart\Views\UserChartFilter.cshtml"
    using Signum.Web.Chart;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Chart\Views\UserChartFilter.cshtml"
    using Signum.Web.UserAssets;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Chart/Views/UserChartFilter.cshtml")]
    public partial class UserChartFilter : System.Web.Mvc.WebViewPage<dynamic>
    {
        public UserChartFilter()
        {
        }
        public override void Execute()
        {
            
            #line 8 "..\..\Chart\Views\UserChartFilter.cshtml"
 using (var e = Html.TypeContext<QueryFilterDN>())
{
    var parent = ((TypeContext<UserChartDN>)e.Parent.Parent).Value;
    e.FormGroupStyle = FormGroupStyle.None;
    
            
            #line default
            #line hidden
            
            #line 12 "..\..\Chart\Views\UserChartFilter.cshtml"
Write(Html.QueryTokenDNBuilder(e.SubContext(qf => qf.Token), ChartClient.GetQueryTokenBuilderSettings(
            (QueryDescription)ViewData[ViewDataKeys.QueryDescription], SubTokensOptions.CanAnyAll | SubTokensOptions.CanElement | (parent.GroupResults ? SubTokensOptions.CanAggregate :  0))));

            
            #line default
            #line hidden
            
            #line 13 "..\..\Chart\Views\UserChartFilter.cshtml"
                                                                                                                                                                                              
    
            
            #line default
            #line hidden
            
            #line 14 "..\..\Chart\Views\UserChartFilter.cshtml"
Write(Html.ValueLine(e, f => f.Operation));

            
            #line default
            #line hidden
            
            #line 14 "..\..\Chart\Views\UserChartFilter.cshtml"
                                        
    
            
            #line default
            #line hidden
            
            #line 15 "..\..\Chart\Views\UserChartFilter.cshtml"
Write(Html.ValueLine(e, f => f.ValueString, vl => vl.ValueHtmlProps["size"] = 20));

            
            #line default
            #line hidden
            
            #line 15 "..\..\Chart\Views\UserChartFilter.cshtml"
                                                                                
}
            
            #line default
            #line hidden
WriteLiteral(" ");

        }
    }
}
#pragma warning restore 1591
