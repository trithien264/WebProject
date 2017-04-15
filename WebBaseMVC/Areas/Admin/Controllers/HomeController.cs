using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBaseMVC.Areas.Admin.Models.Bus;

namespace WebBaseMVC.Areas.Admin.Controllers
{
    public class HomeController : AuthorizeController
    {
        
        //
        // GET: /Admin/Home/
        public ActionResult Index()
        {
           
            //ViewBag.MainMenu = new BusMenu().GetMainMenu();
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult MainMenu()
        {           
            var res= new BusMenu().GetMainMenu();
            return PartialView(res);            
        }




	}
}