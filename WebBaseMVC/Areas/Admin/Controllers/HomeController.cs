using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBaseMVC.Areas.Admin.Controllers
{
    public class HomeController : AuthorizeController
    {
        //
        // GET: /Admin/Home/
        public ActionResult Index()
        {
            return View(); 
        }
	}
}