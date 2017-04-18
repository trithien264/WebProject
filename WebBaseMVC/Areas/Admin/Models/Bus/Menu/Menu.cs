using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBaseMVC.Areas.Admin.Models.Framework;

namespace WebBaseMVC.Areas.Admin.Models.Bus
{
    /*public class Menu
    {
        public int menu_id { get; set; }
        public string menu_nm { get; set; }
        public int? up_menu_id { get; set; }
        public string link { get; set; }        
        public string MenuArea { get; set; }
        public IEnumerable<Menu> ChildMenu { get; set; }
    }*/

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
        public IQueryable<Menu> ChildMenu { get; set; }
    }
}