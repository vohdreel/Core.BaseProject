using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BaseProject.DAO.Model;

namespace BaseProject.DAO.Context
{
    public partial class GlobalContext : DbContext
    {
        public GlobalContext()
        {
        }

        public GlobalContext(DbContextOptions<GlobalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Machine> Machine { get; set; }
        public virtual DbSet<MechaUser> MechaUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-C344KI6;Database=MachineDatabase;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Machine>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Machine)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Machine_User");
            });

            modelBuilder.Entity<MechaUser>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
