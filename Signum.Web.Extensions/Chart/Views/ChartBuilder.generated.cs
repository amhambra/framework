﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Signum.Web.Extensions.Chart.Views
{
    using System;
    using System.Collections.Generic;
    
    #line 5 "..\..\Chart\Views\ChartBuilder.cshtml"
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
    
    #line 11 "..\..\Chart\Views\ChartBuilder.cshtml"
    using Signum.Engine.Chart;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Chart\Views\ChartBuilder.cshtml"
    using Signum.Engine.DynamicQuery;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Chart\Views\ChartBuilder.cshtml"
    using Signum.Entities;
    
    #line default
    #line hidden
    
    #line 8 "..\..\Chart\Views\ChartBuilder.cshtml"
    using Signum.Entities.Chart;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Chart\Views\ChartBuilder.cshtml"
    using Signum.Entities.DynamicQuery;
    
    #line default
    #line hidden
    
    #line 6 "..\..\Chart\Views\ChartBuilder.cshtml"
    using Signum.Entities.Reflection;
    
    #line default
    #line hidden
    
    #line 10 "..\..\Chart\Views\ChartBuilder.cshtml"
    using Signum.Utilities;
    
    #line default
    #line hidden
    
    #line 1 "..\..\Chart\Views\ChartBuilder.cshtml"
    using Signum.Web;
    
    #line default
    #line hidden
    
    #line 9 "..\..\Chart\Views\ChartBuilder.cshtml"
    using Signum.Web.Chart;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Chart\Views\ChartBuilder.cshtml"
    using Signum.Web.Extensions.Properties;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.5.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Chart/Views/ChartBuilder.cshtml")]
    public class ChartBuilder : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ChartBuilder()
        {
        }
        public override void Execute()
        {












            
            #line 12 "..\..\Chart\Views\ChartBuilder.cshtml"
 using (var chart = Html.TypeContext<IChartBase>())
{
    QueryDescription queryDescription = (QueryDescription)ViewData[ViewDataKeys.QueryDescription];
   
    

            
            #line default
            #line hidden
WriteLiteral("    <table id=\"");


            
            #line 17 "..\..\Chart\Views\ChartBuilder.cshtml"
          Write(chart.Compose("sfChartBuilder"));

            
            #line default
            #line hidden
WriteLiteral("\" class=\"sf-chart-builder\" data-url=\"");


            
            #line 17 "..\..\Chart\Views\ChartBuilder.cshtml"
                                                                                Write(Url.Action<ChartController>(cc => cc.UpdateChartBuilder(chart.Parent.ControlID)));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n        <tr>\r\n            <td class=\"ui-widget ui-widget-content ui-corner-al" +
"l sf-chart-type\">\r\n                <div class=\"ui-widget-header\">\r\n             " +
"       ");


            
            #line 21 "..\..\Chart\Views\ChartBuilder.cshtml"
               Write(typeof(ChartScriptDN).NiceName());

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 22 "..\..\Chart\Views\ChartBuilder.cshtml"
                     using (var csc = chart.SubContext(c => c.ChartScript))
                    {
                        
            
            #line default
            #line hidden
            
            #line 24 "..\..\Chart\Views\ChartBuilder.cshtml"
                   Write(Html.Hidden(csc.Compose("sfRuntimeInfo"), csc.RuntimeInfo().ToString(), new { @class = "sf-chart-type-value" }));

            
            #line default
            #line hidden
            
            #line 24 "..\..\Chart\Views\ChartBuilder.cshtml"
                                                                                                                                        
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");


            
            #line 26 "..\..\Chart\Views\ChartBuilder.cshtml"
               Write(Html.Hidden(chart.Compose("GroupResults"), chart.Value.GroupResults, new { @class = "sf-chart-group-results" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");


            
            #line 28 "..\..\Chart\Views\ChartBuilder.cshtml"
                 foreach (var group in ChartUtils.PackInGroups(ChartScriptLogic.Scripts.Value, 4))
                {   
                    foreach (var script in group)
                    { 

            
            #line default
            #line hidden
WriteLiteral("                    <div class=\"");


            
            #line 32 "..\..\Chart\Views\ChartBuilder.cshtml"
                           Write(ChartClient.ChartTypeImgClass(chart.Value, chart.Value.ChartScript, script));

            
            #line default
            #line hidden
WriteLiteral("\" \r\n                    data-related=\"");


            
            #line 33 "..\..\Chart\Views\ChartBuilder.cshtml"
                              Write(new RuntimeInfo(script).ToString());

            
            #line default
            #line hidden
WriteLiteral("\"  title=\"");


            
            #line 33 "..\..\Chart\Views\ChartBuilder.cshtml"
                                                                             Write(script.ToString() + "\r\n" + script.ColumnsStructure);

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                        <img src=\"");


            
            #line 34 "..\..\Chart\Views\ChartBuilder.cshtml"
                              Write(script.Icon == null ? Url.Content("~/Chart/Images/unknown.png") :
                        Url.Action((Signum.Web.Files.FileController fc) => fc.Download(new RuntimeInfo(script.Icon).ToString())));

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n                    </div>\r\n");


            
            #line 37 "..\..\Chart\Views\ChartBuilder.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    <div class=\"clearall\">\r\n                    </div>\r\n");


            
            #line 40 "..\..\Chart\Views\ChartBuilder.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </td>\r\n            <td class=\"ui-widget ui-widget-content ui-corner-a" +
"ll sf-chart-tokens\">\r\n                <div class=\"ui-widget-header\">");


            
            #line 43 "..\..\Chart\Views\ChartBuilder.cshtml"
                                         Write(Resources.Chart_ChartSettings);

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                <table>\r\n                    <tr>\r\n                      " +
"  <th class=\"sf-chart-token-narrow\">");


            
            #line 46 "..\..\Chart\Views\ChartBuilder.cshtml"
                                                     Write(Resources.Chart_Dimension);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </th>\r\n                        <th class=\"sf-chart-toke" +
"n-narrow\">");


            
            #line 48 "..\..\Chart\Views\ChartBuilder.cshtml"
                                                     Write(Resources.Chart_Group);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </th>\r\n                        <th class=\"sf-chart-toke" +
"n-wide\">\r\n                            Token\r\n                        </th>\r\n    " +
"                </tr>\r\n");


            
            #line 54 "..\..\Chart\Views\ChartBuilder.cshtml"
                     foreach (var column in chart.TypeElementContext(a => a.Columns))
                    {
                        
            
            #line default
            #line hidden
            
            #line 56 "..\..\Chart\Views\ChartBuilder.cshtml"
                   Write(Html.HiddenRuntimeInfo(column));

            
            #line default
            #line hidden
            
            #line 56 "..\..\Chart\Views\ChartBuilder.cshtml"
                                                       
                        
            
            #line default
            #line hidden
            
            #line 57 "..\..\Chart\Views\ChartBuilder.cshtml"
                   Write(Html.EmbeddedControl(column, c => c, ec => ec.ViewData[ViewDataKeys.QueryDescription] = queryDescription));

            
            #line default
            #line hidden
            
            #line 57 "..\..\Chart\Views\ChartBuilder.cshtml"
                                                                                                                                  
                    }

            
            #line default
            #line hidden
WriteLiteral("                </table>\r\n                <textarea class=\"sf-chart-currentScript" +
"\" style=\"display:none\" data-url=\"");


            
            #line 60 "..\..\Chart\Views\ChartBuilder.cshtml"
                                                                                   Write(Navigator.NavigateRoute(chart.Value.ChartScript));

            
            #line default
            #line hidden
WriteLiteral("\">\r\n                    ");


            
            #line 61 "..\..\Chart\Views\ChartBuilder.cshtml"
               Write(chart.Value.ChartScript.Script);

            
            #line default
            #line hidden
WriteLiteral("\r\n                </textarea>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n");


            
            #line 66 "..\..\Chart\Views\ChartBuilder.cshtml"
}
            
            #line default
            #line hidden

        }
    }
}
#pragma warning restore 1591
