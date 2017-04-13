using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebBaseMVC.Models.CustomModel;

namespace WebBaseMVC.Areas.Admin.Controllers
{
    public class AuthorizeController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[CmHelper.AutHelper.LoginUserKey];
            if (session == null)
            {
                //filterContext.Result= new RedirectToRouteResult(new RouteValueDictionary(new {controller="Login", action="Index",Area="Admin"}));
                ReturnHomePage(filterContext);
            }
            else // is login
            {
                List<KeyValuePair<string, string>> validMethod = new List<KeyValuePair<string, string>>(); //Action/Controller
                validMethod.Add(new KeyValuePair<string, string>("Index", "Home"));

                var result = validMethod.Where(r => r.Key == filterContext.ActionDescriptor.ActionName && r.Value == filterContext.ActionDescriptor.ControllerDescriptor.ControllerName).Count();
                if (result < 1)
                {
                    ReturnHomePage(filterContext);
                }

            }

            base.OnActionExecuting(filterContext);

        }

        public void ReturnHomePage(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
        }
    }
}