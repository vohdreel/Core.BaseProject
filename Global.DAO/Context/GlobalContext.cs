using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Global.DAO.Model;
using Global.DAO.Procedure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Global.DAO.Context
{
    public partial class GlobalContext : IdentityDbContext
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
                optionsBuilder.UseSqlServer("Server=prolead.database.windows.net;Database=GlobalEmpregos;user id=anima_sa;password=A^BCxSFd#%qHv=W79uda;Trusted_Connection=True;Integrated Security=False;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MachineUser>().HasNoKey();

            modelBuilder.Entity<Machine>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.IdMechaUserNavigation)
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
