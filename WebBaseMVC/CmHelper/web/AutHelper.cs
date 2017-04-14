using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBaseMVC.Areas.Admin.Models.Framework;
using WebBaseMVC.Models.CustomModel;

namespace WebBaseMVC.CmHelper
{
    public class AutHelper
    {
        private static readonly AutHelper _instance = new AutHelper();
  
        static AutHelper()
        {
        }

        public static AutHelper Instance
        {
            get
            {
                return _instance;
            }
        }       

        public static UserLogin userLogin
        {
            get
            {
                return (UserLogin)System.Web.HttpContext.Current.Session[CmHelper.AutHelper.LoginUserKey];
            }
            set {
                System.Web.HttpContext.Current.Session.Add(AutHelper.LoginUserKey, value); 
            }
        }   

        public const string LoginUserKey = "AutHelper_LoginUserKey";
        WebBaseDbContext dbcontext = null;
        public UserLogin Login(string UserName,string Password)
        {
            dbcontext = new WebBaseDbContext();
            var res = dbcontext.base_user.Where(m => m.email == UserName && m.pwd == Password).FirstOrDefault();
            if (res!=null)
            {
                return new UserLogin() { UserID = res.user_id, UserName = res.email };
            }
            return null;
        }
    }
}