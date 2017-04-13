namespace WebBaseMVC.Areas.Admin.Models.Framework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WebBaseDbContext : DbContext
    {
        public WebBaseDbContext()
            : base("name=WebBaseDbContext")
        {
        }

        public virtual DbSet<base_user> base_user { get; set; }
        public virtual DbSet<base_userrights> base_userrights { get; set; }
        public virtual DbSet<base_menu> base_menu { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<base_user>()
                .Property(e => e.user_id)
                .HasPrecision(18, 0);

            modelBuilder.Entity<base_user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<base_user>()
                .Property(e => e.pwd)
                .IsUnicode(false);

            modelBuilder.Entity<base_user>()
                .Property(e => e.end_mk)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<base_user>()
                .Property(e => e.upd_id)
                .HasPrecision(18, 0);

            modelBuilder.Entity<base_userrights>()
                .Property(e => e.ur_id)
                .HasPrecision(18, 0);

            modelBuilder.Entity<base_userrights>()
                .Property(e => e.user_id)
                .HasPrecision(18, 0);

            modelBuilder.Entity<base_userrights>()
                .Property(e => e.right_kind)
                .IsUnicode(false);

            modelBuilder.Entity<base_menu>()
                .Property(e => e.link)
                .IsUnicode(false);

            modelBuilder.Entity<base_menu>()
                .Property(e => e.group_type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<base_menu>()
                .Property(e => e.link_kind)
                .IsUnicode(false);

            modelBuilder.Entity<base_menu>()
                .Property(e => e.stop_mk)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<base_menu>()
                .Property(e => e.icon)
                .IsUnicode(false);
        }
    }
}
