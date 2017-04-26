using BusService.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBaseMVC.Areas.Admin.Models.Framework;

namespace WebBaseMVC.API.Admin.User
{
    public class UserManage : Controller
    {
        WebBaseDbContext dbcontext = null;
        public List<base_user> GetListUserAll()
        {           
            dbcontext = new WebBaseDbContext();   
            return dbcontext.base_user.ToList();     
        }


        public string UpdateUser(Dictionary<string, object> dic)
        {
            //string json=JsonConvert.SerializeObject(dic);
            //base_user model = JsonConvert.DeserializeObject<base_user>(json);  

            base_user model = Tool.ToObject<base_user>(dic);
            dbcontext = new WebBaseDbContext();            

            if(ModelState.IsValid)
            {
                dbcontext = new WebBaseDbContext();
                var user = dbcontext.base_user.Where(u => u.user_id == model.user_id).First();
                user.user_desc = model.user_desc;               
                user.email = model.email;
                user.end_mk = model.end_mk;
                user.upd_date = model.upd_date;                          
                dbcontext.SaveChanges();              
            };
            
            return "OK";
        }
    }
}