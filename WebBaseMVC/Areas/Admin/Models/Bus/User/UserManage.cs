using BusService.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBaseMVC.Areas.Admin.Models.Framework;

namespace WebBaseMVC.Areas.Admin.Models.Bus
{
    public class UserManage : Controller
    {
        WebBaseDbContext dbcontext = null;
        public List<base_user> GetListUserAll(base_user model)
        {           
            dbcontext = new WebBaseDbContext();   
            return dbcontext.base_user.ToList();     
        }

        public string SaveUser(object model)
        {            
            dbcontext = new WebBaseDbContext();
            
            /*if(ModelState.IsValid)
            {
                dbcontext = new WebBaseDbContext();
                var user = dbcontext.base_user.Where(u => u.user_id == model.user_id).First();
                user.user_desc = model.user_desc;
               
                user.email = model.email;
                user.end_mk = model.end_mk;
                user.upd_date = model.upd_date;

                          
                dbcontext.SaveChanges();              

            };

            List<SelectListItem> items = new List<SelectListItem>(){
                new SelectListItem(){Text = "Sử dụng", Value = "N"},
                new SelectListItem { Text = "Ngưng sử dụng", Value = "Y" }
            };
            ViewBag.TrangThai = items;*/
            return "OK";
        }
    }
}