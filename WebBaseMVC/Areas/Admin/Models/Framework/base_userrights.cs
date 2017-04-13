namespace WebBaseMVC.Areas.Admin.Models.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class base_userrights
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ur_id { get; set; }

        public int? menu_id { get; set; }

        public decimal? user_id { get; set; }

        [StringLength(10)]
        public string right_kind { get; set; }
    }
}
