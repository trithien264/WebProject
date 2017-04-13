namespace WebBaseMVC.Areas.Admin.Models.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class base_user
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal user_id { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(100)]
        public string pwd { get; set; }

        [StringLength(100)]
        public string user_desc { get; set; }

        [StringLength(1)]
        public string end_mk { get; set; }

        public decimal? upd_id { get; set; }

        public DateTime? upd_date { get; set; }

        public DateTime? last_login { get; set; }
    }
}
