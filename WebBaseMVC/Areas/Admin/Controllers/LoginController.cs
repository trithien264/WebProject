using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBaseMVC.Areas.Admin.Models.Framework;
using WebBaseMVC.CmHelper;
using WebBaseMVC.Models.CustomModel;


namespace WebBaseMVC.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        WebBaseDbContext dbcontext = null;
        //
        // GET: /Login/
        [HttpGet]
        public ActionResult Index()
        {                 
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove(AutHelper.LoginUserKey);
            return RedirectToAction("Index", "Login");            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                dbcontext = new WebBaseDbContext();
                UserLogin user = AutHelper.Instance.Login(model.UserName, model.Password);
                if(user!=null)
                {
                    Session.Add(AutHelper.LoginUserKey, user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại !!!");
                }

            }
            return View();
        }
    }
}