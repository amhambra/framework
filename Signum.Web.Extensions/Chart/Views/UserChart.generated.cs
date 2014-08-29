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
    
    #line 4 "..\..\Chart\Views\UserChart.cshtml"
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
    
    #line 9 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Engine.Basics;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Engine.DynamicQuery;
    
    #line default
    #line hidden
    
    #line 6 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Entities;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Entities.Chart;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Entities.DynamicQuery;
    
    #line default
    #line hidden
    
    #line 5 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Entities.Reflection;
    
    #line default
    #line hidden
    
    #line 11 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Entities.UserAssets;
    
    #line default
    #line hidden
    
    #line 10 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Entities.UserQueries;
    
    #line default
    #line hidden
    using Signum.Utilities;
    
    #line 1 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Web;
    
    #line default
    #line hidden
    
    #line 8 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Web.Chart;
    
    #line default
    #line hidden
    
    #line 12 "..\..\Chart\Views\UserChart.cshtml"
    using Signum.Web.UserAssets;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Chart/Views/UserChart.cshtml")]
    public partial class UserChart : System.Web.Mvc.WebViewPage<dynamic>
    {
        public UserChart()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 14 "..\..\Chart\Views\UserChart.cshtml"
Write(Html.ScriptCss("~/Chart/Content/Chart.css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"sf-chart-control\"");

WriteLiteral(" \r\n    data-subtokens-url=\"");

            
            #line 16 "..\..\Chart\Views\UserChart.cshtml"
                   Write(Url.Action("NewSubTokensCombo", "Chart"));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" \r\n    data-add-filter-url=\"");

            
            #line 17 "..\..\Chart\Views\UserChart.cshtml"
                    Write(Url.Action("AddFilter", "Chart"));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" \r\n    data-prefix=\"");

            
            #line 18 "..\..\Chart\Views\UserChart.cshtml"
            Write(Model.Prefix);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n");

            
            #line 19 "..\..\Chart\Views\UserChart.cshtml"
    
            
            #line default
            #line hidden
            
            #line 19 "..\..\Chart\Views\UserChart.cshtml"
     using (var uc = Html.TypeContext<UserChartDN>())
    {
        uc.LabelColumns = new BsColumn(2);

        object queryName = QueryLogic.ToQueryName(uc.Value.Query.Key);

        QueryDescription queryDescription = (QueryDescription)ViewData[ViewDataKeys.QueryDescription];
        if (queryDescription == null)
        {
            queryDescription = DynamicQueryManager.Current.QueryDescription(queryName);
            ViewData[ViewDataKeys.QueryDescription] = queryDescription;
        }
        
        
            
            #line default
            #line hidden
            
            #line 32 "..\..\Chart\Views\UserChart.cshtml"
   Write(Html.Hidden("webQueryName", Navigator.ResolveWebQueryName(queryName)));

            
            #line default
            #line hidden
            
            #line 32 "..\..\Chart\Views\UserChart.cshtml"
                                                                              

        using (var query = uc.SubContext(tc => tc.Query))
        {
        
            
            #line default
            #line hidden
            
            #line 36 "..\..\Chart\Views\UserChart.cshtml"
   Write(Html.HiddenRuntimeInfo(query));

            
            #line default
            #line hidden
            
            #line 36 "..\..\Chart\Views\UserChart.cshtml"
                                      
        
        
            
            #line default
            #line hidden
            
            #line 38 "..\..\Chart\Views\UserChart.cshtml"
   Write(Html.Hidden(query.Compose("Key"), query.Value.Key));

            
            #line default
            #line hidden
            
            #line 38 "..\..\Chart\Views\UserChart.cshtml"
                                                           
        
            
            #line default
            #line hidden
            
            #line 39 "..\..\Chart\Views\UserChart.cshtml"
   Write(Html.Hidden(query.Compose("Name"), query.Value.Name));

            
            #line default
            #line hidden
            
            #line 39 "..\..\Chart\Views\UserChart.cshtml"
                                                             

        var niceName = QueryUtils.GetNiceName(query.Value.ToQueryName());
        
        
            
            #line default
            #line hidden
            
            #line 43 "..\..\Chart\Views\UserChart.cshtml"
   Write(Html.FormGroup(uc, null, typeof(Signum.Entities.Basics.QueryDN).NiceName(), Navigator.IsFindable(queryName) ?
                new HtmlTag("a").Class("form-control-static").Attr("href", Navigator.FindRoute(queryName)).SetInnerText(niceName).ToHtml() :
                Html.Span(null, niceName, "form-control-static")));

            
            #line default
            #line hidden
            
            #line 45 "..\..\Chart\Views\UserChart.cshtml"
                                                                 
        }
    
        
            
            #line default
            #line hidden
            
            #line 48 "..\..\Chart\Views\UserChart.cshtml"
   Write(Html.EntityLine(uc, tc => tc.Owner, el => el.Create = false));

            
            #line default
            #line hidden
            
            #line 48 "..\..\Chart\Views\UserChart.cshtml"
                                                                     
        
            
            #line default
            #line hidden
            
            #line 49 "..\..\Chart\Views\UserChart.cshtml"
   Write(Html.ValueLine(uc, tc => tc.DisplayName));

            
            #line default
            #line hidden
            
            #line 49 "..\..\Chart\Views\UserChart.cshtml"
                                                 
        
        var entityTypePrefix = uc.SubContext(a => a.EntityType).Prefix;
        
        
            
            #line default
            #line hidden
            
            #line 53 "..\..\Chart\Views\UserChart.cshtml"
   Write(Html.EntityLine(uc, f => f.EntityType, el =>
        {
            el.AutocompleteUrl = Url.Action("TypeAutocomplete", "Finder");
            el.AttachFunction = UserAssetsClient.Module["attachShowCurrentEntity"](el);
        }));

            
            #line default
            #line hidden
            
            #line 57 "..\..\Chart\Views\UserChart.cshtml"
          


            
            #line default
            #line hidden
WriteLiteral("        <p");

WriteLiteral(" class=\"messageEntity col-sm-offset-2\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 60 "..\..\Chart\Views\UserChart.cshtml"
       Write(UserQueryMessage.Use0ToFilterCurrentEntity.NiceToString().Formato(CurrentEntityConverter.CurrentEntityKey));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </p>\r\n");

            
            #line 62 "..\..\Chart\Views\UserChart.cshtml"
        
        using (var sc = uc.SubContext())
        {
            sc.FormGroupSize = FormGroupSize.Small;
            

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"repeater-inline form-inline sf-filters-list \"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 68 "..\..\Chart\Views\UserChart.cshtml"
       Write(Html.EntityRepeater(uc, tc => tc.Filters, er => { er.PreserveViewData = true; er.PartialViewName = "~/Chart/Views/UserChartFilter.cshtml"; }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n");

            
            #line 70 "..\..\Chart\Views\UserChart.cshtml"


            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"repeater-inline form-inline sf-filters-list\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 72 "..\..\Chart\Views\UserChart.cshtml"
       Write(Html.EntityRepeater(uc, tc => tc.Orders, er => { er.PreserveViewData = true; er.PartialViewName = "~/Chart/Views/UserChartOrder.cshtml"; }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n");

            
            #line 74 "..\..\Chart\Views\UserChart.cshtml"
        }
    

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteAttribute("id", Tuple.Create(" id=\"", 3171), Tuple.Create("\"", 3214)
            
            #line 76 "..\..\Chart\Views\UserChart.cshtml"
, Tuple.Create(Tuple.Create("", 3176), Tuple.Create<System.Object, System.Int32>(uc.Compose("sfChartBuilderContainer")
            
            #line default
            #line hidden
, 3176), false)
);

WriteLiteral(" class=\"SF-control-container\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 77 "..\..\Chart\Views\UserChart.cshtml"
       Write(Html.Partial(ChartClient.ChartBuilderView, uc));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n");

            
            #line 79 "..\..\Chart\Views\UserChart.cshtml"
        

            
            #line default
            #line hidden
WriteLiteral("        <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n            $(function () {\r\n                require([\"");

            
            #line 82 "..\..\Chart\Views\UserChart.cshtml"
                     Write(ChartClient.Module);

            
            #line default
            #line hidden
WriteLiteral("\"], function (Chart) {\r\n                    new Chart.ChartBuilder(");

            
            #line 83 "..\..\Chart\Views\UserChart.cshtml"
                                      Write(MvcHtmlString.Create(uc.Value.ToChartBuilder(Url, uc.Prefix).ToString()));

            
            #line default
            #line hidden
WriteLiteral(");\r\n                });\r\n            });\r\n        </script>\r\n");

            
            #line 87 "..\..\Chart\Views\UserChart.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");

        }
    }
}
#pragma warning restore 1591
