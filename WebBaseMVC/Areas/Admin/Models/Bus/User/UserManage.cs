using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBaseMVC.Areas.Admin.Models.Framework;

namespace WebBaseMVC.Areas.Admin.Models.Bus
{
    public class UserManage
    {
        WebBaseDbContext dbcontext = null;
        public List<base_user> GetListUserAll()
        {           
            dbcontext = new WebBaseDbContext();   
            return dbcontext.base_user.ToList();     
        }
    }
}