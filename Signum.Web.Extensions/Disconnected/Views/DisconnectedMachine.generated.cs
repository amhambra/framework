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
    
    #line 1 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
    using Signum.Entities.Disconnected;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
    using Signum.Entities.DynamicQuery;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Disconnected/Views/DisconnectedMachine.cshtml")]
    public partial class _Disconnected_Views_DisconnectedMachine_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Disconnected_Views_DisconnectedMachine_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 4 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
 using (var dc = Html.TypeContext<DisconnectedMachineEntity>())
{
    
            
            #line default
            #line hidden
            
            #line 6 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
Write(Html.ValueLine(dc, d => d.CreationDate));

            
            #line default
            #line hidden
            
            #line 6 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
                                            
    
            
            #line default
            #line hidden
            
            #line 7 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
Write(Html.ValueLine(dc, d => d.MachineName));

            
            #line default
            #line hidden
            
            #line 7 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
                                           
    
            
            #line default
            #line hidden
            
            #line 8 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
Write(Html.ValueLine(dc, d => d.State, vl => vl.ReadOnly = true));

            
            #line default
            #line hidden
            
            #line 8 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
                                                               
    
            
            #line default
            #line hidden
            
            #line 9 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
Write(Html.ValueLine(dc, d => d.SeedMin));

            
            #line default
            #line hidden
            
            #line 9 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
                                       
    
            
            #line default
            #line hidden
            
            #line 10 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
Write(Html.ValueLine(dc, d => d.SeedMax));

            
            #line default
            #line hidden
            
            #line 10 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
                                       

    if (!dc.Value.IsNew)
    {

            
            #line default
            #line hidden
WriteLiteral("        <fieldset>\r\n            <legend>Last Imports</legend>\r\n");

WriteLiteral("            ");

            
            #line 16 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
       Write(Html.SearchControl(new FindOptions(typeof(DisconnectedImportEntity), "Machine", dc.Value)
       {
          OrderOptions = { new OrderOption("CreationDate", OrderType.Descending) },
      }, dc));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </fieldset>\r\n");

            
            #line 21 "..\..\Disconnected\Views\DisconnectedMachine.cshtml"
    }
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
