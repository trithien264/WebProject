namespace WebBaseMVC.Areas.Admin.Models.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class base_menu
    {
        [Key]
        public int menu_id { get; set; }

        [StringLength(100)]
        public string menu_nm { get; set; }

        [StringLength(100)]
        public string menu_nm_eng { get; set; }

        public int? up_menu_id { get; set; }

        [StringLength(500)]
        public string link { get; set; }

        [StringLength(1)]
        public string group_type { get; set; }

        [StringLength(10)]
        public string link_kind { get; set; }

        [StringLength(1)]
        public string stop_mk { get; set; }

        [StringLength(100)]
        public string icon { get; set; }
        [NotMapped]
        public  List<base_menu> ChildMenu { get; set; }
    }
}
