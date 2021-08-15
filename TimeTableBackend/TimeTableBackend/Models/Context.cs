using System;
using Microsoft.EntityFrameworkCore;

namespace TimeTableBackend.Models
{
    public class Context: DbContext
    {
        public Context()
        {
        }

        public virtual DbSet<NienKhoa> NienKhoas  { get; set; }
        public virtual DbSet<MonHoc> MonHocs{ get; set; }
        public virtual DbSet<NhomMonHoc> NhomMonHocs{ get; set; }
        public virtual DbSet<Buoi> Buois{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseLazyLoadingProxies().UseSqlite("Data Source=db.db");
                optionsBuilder.UseSqlite("Data Source=db.db");

            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NienKhoa>()
            .HasMany(g => g.MonHocs).WithOne(s => s.NienKhoa).HasForeignKey(s=>s.NienKhoaId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MonHoc>()
            .HasMany(g => g.NhomMonHoc).WithOne(s => s.MonHoc).HasForeignKey(s => s.MonHocId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NhomMonHoc>()
            .HasMany(g => g.Buois).WithOne(s => s.NhomMonHoc).HasForeignKey(s => s.NhomMonHocId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
