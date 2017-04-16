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
        public class Menu
        {
            public int menu_id { get; set; }           
            public string menu_nm { get; set; }
            public string menu_nm_eng { get; set; }
            public int? up_menu_id { get; set; }
            public string link { get; set; }
            public string group_type { get; set; }
            public string link_kind { get; set; }
            public string stop_mk { get; set; }
            public string icon { get; set; }
            public List<Menu> ChildMenu { get; set; }
        }
        public List<base_menu> GetMainMenu()
        {
            dbcontext = new WebBaseDbContext();
            return dbcontext.base_menu.ToList();

            /*IQueryable<base_menu> query1 = from row in dbcontext.base_menu
                        select new base_menu
                        {
                            menu_id = row.menu_id,
                            up_menu_id = row.up_menu_id,
                            menu_nm = row.menu_nm,                            
                            link = row.link,
                           // ChildMenu = dbcontext.base_menu.Where(x => x.up_menu_id == row.menu_id).ToList()
                        };*/


            //IQueryable<Menu> q = from row in dbcontext.base_menu
            //                        select new Menu
            //                        {
            //                            menu_id = row.menu_id,
            //                            up_menu_id = row.up_menu_id,
            //                            menu_nm = row.menu_nm,
            //                            link = row.link,
            //                            ChildMenu = dbcontext.base_menu.Where(x => x.up_menu_id == row.menu_id).ToList<Menu>()
            //                        };

            //List<Menu> l = new List<Menu>(q);
            //return l;
        }
    }
}