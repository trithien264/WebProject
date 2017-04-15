using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBaseMVC.Areas.Admin.Models.Framework;

namespace WebBaseMVC.Areas.Admin.Models.Bus
{    
    public class BusMenu
    {
        WebBaseDbContext dbcontext = null;

        public IEnumerable<Menu> GetMainMenu()
        {
            /*dbcontext = new WebBaseDbContext();
            return dbcontext.base_menu.ToList();*/

            var query = from row in dbcontext.base_menu
                        select new Menu
                        {
                            menu_id = row.menu_id,
                            up_menu_id = row.up_menu_id,
                            menu_nm = row.menu_nm,                            
                            link = row.link,
                            ChildMenu = dbcontext.base_menu.Where(x => x.up_menu_id == row.menu_id).ToList()
                        };

            return query;
        }
    }
}