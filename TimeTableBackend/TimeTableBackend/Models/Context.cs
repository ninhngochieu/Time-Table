using System;
using Microsoft.EntityFrameworkCore;

namespace TimeTableBackend.Models
{
    public class Context: DbContext
    {
        public Context()
        {
        }

        public virtual DbSet<MonHoc> MonHocs{ get; set; }
        public virtual DbSet<NhomMonHoc> NhomMonHocs{ get; set; }
        public virtual DbSet<Buoi> Buois{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=db.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
