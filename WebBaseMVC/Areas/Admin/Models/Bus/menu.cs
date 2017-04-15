using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBaseMVC.Areas.Admin.Models.Framework;

namespace WebBaseMVC.Areas.Admin.Models.Bus
{
    public class Menu
    {
        public int menu_id { get; set; }
        public string menu_nm { get; set; }
        public int? up_menu_id { get; set; }
        public string link { get; set; }        
        public string MenuArea { get; set; }
        public IList<base_menu> ChildMenu { get; set; }
    }
}