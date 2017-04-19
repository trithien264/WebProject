using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBaseMVC.Areas.Admin.Models.Framework;

namespace WebBaseMVC.Areas.Admin.Controllers
{
    public class UserManageController : Controller
    {
        //
        // GET: /Admin/UserManage/
        public ActionResult Index()
        {
            return View();
        }
        WebBaseDbContext dbcontext = null;
        public string SaveUserInfo(base_user user)
        {
            dbcontext = new WebBaseDbContext();
            return "aaaaaaaaaa";
        }
	}
}