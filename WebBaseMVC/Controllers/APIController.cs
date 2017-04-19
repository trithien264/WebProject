using BusService;
using BusService.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebBaseMVC.Areas.Admin.Models.Framework;

namespace WebBaseMVC.Controllers
{
    public class APIController : Controller
    {
        public JsonResult Run(string JsService)
        {            
            return Json(ServiceCaller.Instance.CallToDic(ServiceCaller.CallType.BaseCall, JsService));
        }
    } 
}