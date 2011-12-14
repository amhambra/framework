﻿#region usings
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Signum.Services;
using Signum.Utilities;
using Signum.Entities;
using Signum.Web;
using Signum.Engine;
using Signum.Engine.Operations;
using Signum.Entities.Operations;
using Signum.Engine.Basics;
using Signum.Web.Extensions.Properties;
using Signum.Entities.Processes;
using Signum.Engine.Processes;
using Signum.Engine.Authorization;
#endregion

namespace Signum.Web.Processes
{
    public class ProcessController : Controller
    {
        [HttpPost]
        public ContentResult GetProgressExecution(int id)
        {
            decimal progress = Database.Query<ProcessExecutionDN>().Where(pe => 
                    pe.Id == id).Select(pe => pe.Progress).SingleEx() ?? 100;

            return Content(Math.Round(progress, 0).ToString());
        }

        [HttpPost]
        public PartialViewResult FinishProcessNormalPage()
        {
            ProcessExecutionDN process = this.ExtractEntity<ProcessExecutionDN>()
                .ThrowIfNullC("Entity was not possible to Extract");

            return Navigator.NormalControl(this, process);
        }

        [HttpGet]
        public ActionResult View()
        {
            ProcessLogicState state = ProcessLogic.ExecutionState();

            if (Request.IsAjaxRequest())
            {
                return View(ProcessesClient.ViewPrefix.Formato("ProcessPanelTable"), state);
            }
            else
            {
                return PartialView(ProcessesClient.ViewPrefix.Formato("ProcessPanel"), state);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Start()
        {
            ProcessPermissions.ViewProcessControlPanel.Authorize();

            ProcessLogic.Start();
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Stop()
        {
            ProcessPermissions.ViewProcessControlPanel.Authorize();

            ProcessLogic.Stop();
            return null;
        }
    }
}
