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
    
    #line 1 "..\..\Mailing\Views\ClientCertificationFile.cshtml"
    using Signum.Entities.Mailing;
    
    #line default
    #line hidden
    using Signum.Utilities;
    using Signum.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Mailing/Views/ClientCertificationFile.cshtml")]
    public partial class _Mailing_Views_ClientCertificationFile_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Mailing_Views_ClientCertificationFile_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 3 "..\..\Mailing\Views\ClientCertificationFile.cshtml"
 using (var sc = Html.TypeContext<ClientCertificationFileEntity>())
{
    
            
            #line default
            #line hidden
            
            #line 5 "..\..\Mailing\Views\ClientCertificationFile.cshtml"
Write(Html.ValueLine(sc, s => s.CertFileType));

            
            #line default
            #line hidden
            
            #line 5 "..\..\Mailing\Views\ClientCertificationFile.cshtml"
                                            
    
            
            #line default
            #line hidden
            
            #line 6 "..\..\Mailing\Views\ClientCertificationFile.cshtml"
Write(Html.ValueLine(sc, s => s.FullFilePath));

            
            #line default
            #line hidden
            
            #line 6 "..\..\Mailing\Views\ClientCertificationFile.cshtml"
                                            
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
