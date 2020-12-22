using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Global.DAO.Model;
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

        public virtual DbSet<AreaInteresse> AreaInteresse { get; set; }
        public virtual DbSet<Candidato> Candidato { get; set; }
        public virtual DbSet<Candidatura> Candidatura { get; set; }
        public virtual DbSet<Cargo> Cargo { get; set; }
        public virtual DbSet<CargoInteresse> CargoInteresse { get; set; }
        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<EnumAgrupamento> EnumAgrupamento { get; set; }
        public virtual DbSet<EnumTipoDocumento> EnumTipoDocumento { get; set; }
        public virtual DbSet<Notificacao> Notificacao { get; set; }
        public virtual DbSet<ProcessoSeletivo> ProcessoSeletivo { get; set; }
        public virtual DbSet<Telefone> Telefone { get; set; }
        public virtual DbSet<TelefoneCandidato> TelefoneCandidato { get; set; }
        public virtual DbSet<TelefoneEmpresa> TelefoneEmpresa { get; set; }
        public virtual DbSet<Vaga> Vaga { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-C344KI6;Database=GlobalEmpregos;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AreaInteresse>(entity =>
            {
                entity.HasOne(d => d.IdCandidatoNavigation)
                    .WithMany(p => p.AreaInteresse)
                    .HasForeignKey(d => d.IdCandidato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidato_AreaInteresse");

                entity.HasOne(d => d.IdEnumAgrupamentoNavigation)
                    .WithMany(p => p.AreaInteresse)
                    .HasForeignKey(d => d.IdEnumAgrupamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnumAgrupamento_AreaInteresse");
            });


            modelBuilder.Entity<Candidato>(entity =>
            {
                entity.Property(e => e.AmputacaoMembrosInferiores).IsUnicode(false);

                entity.Property(e => e.AmputacaoMembrosSuperiores).IsUnicode(false);

                entity.Property(e => e.Bairro).IsUnicode(false);

                entity.Property(e => e.CEP).IsUnicode(false);

                entity.Property(e => e.CID).IsUnicode(false);

                entity.Property(e => e.CPF).IsUnicode(false);

                entity.Property(e => e.CagoInteresseSecundario).IsUnicode(false);

                entity.Property(e => e.CargoInteresse).IsUnicode(false);

                entity.Property(e => e.CategoriaCNH).IsUnicode(false);

                entity.Property(e => e.Cidade).IsUnicode(false);

                entity.Property(e => e.DeficienciaAudicao).IsUnicode(false);

                entity.Property(e => e.DeficienciaCrescimento).IsUnicode(false);

                entity.Property(e => e.DeficienciaFala).IsUnicode(false);

                entity.Property(e => e.DeficienciasMentais).IsUnicode(false);

                entity.Property(e => e.DisponibilidadeHorario).IsUnicode(false);

                entity.Property(e => e.DisponibilidadeTransferencia).IsUnicode(false);

                entity.Property(e => e.DisponibilidadeViagem).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.EmailSecundario).IsUnicode(false);

                entity.Property(e => e.Endereco).IsUnicode(false);

                entity.Property(e => e.Estado).IsUnicode(false);

                entity.Property(e => e.EstadoCivil).IsUnicode(false);

                entity.Property(e => e.EstadoNascimento).IsUnicode(false);

                entity.Property(e => e.FCMToken)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IDLegado).IsUnicode(false);

                entity.Property(e => e.IdAspNetUsers)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Identidade).IsUnicode(false);

                entity.Property(e => e.IdentidadeEstado).IsUnicode(false);

                entity.Property(e => e.IdentidadePais).IsUnicode(false);

                entity.Property(e => e.LocalPreferencia).IsUnicode(false);

                entity.Property(e => e.LocalPreferenciaSecundario).IsUnicode(false);

                entity.Property(e => e.Nacionalidade).IsUnicode(false);

                entity.Property(e => e.Nome).IsUnicode(false);

                entity.Property(e => e.NomeMae).IsUnicode(false);

                entity.Property(e => e.NomePai).IsUnicode(false);

                entity.Property(e => e.NumeroEcomplemetno).IsUnicode(false);

                entity.Property(e => e.Observacoes).IsUnicode(false);

                entity.Property(e => e.OrgaoEmissor).IsUnicode(false);

                entity.Property(e => e.Pais).IsUnicode(false);

                entity.Property(e => e.PretencaoSalarial).IsUnicode(false);

                entity.Property(e => e.Raca).IsUnicode(false);

                entity.Property(e => e.Sexo).IsUnicode(false);

                entity.Property(e => e.TipoVaga).IsUnicode(false);
            });

            modelBuilder.Entity<Candidatura>(entity =>
            {
                entity.HasOne(d => d.IdCandidatoNavigation)
                    .WithMany(p => p.Candidatura)
                    .HasForeignKey(d => d.IdCandidato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidato_Candidatura");

                entity.HasOne(d => d.IdVagaNavigation)
                    .WithMany(p => p.Candidatura)
                    .HasForeignKey(d => d.IdVaga)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vaga_Candidatura");
            });

            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.Property(e => e.DescricaoCargo).IsUnicode(false);

                entity.Property(e => e.NomeCargo).IsUnicode(false);

                entity.HasOne(d => d.IdEnumAgrupamentoNavigation)
                    .WithMany(p => p.Cargo)
                    .HasForeignKey(d => d.IdEnumAgrupamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnumAgrupamento_Cargo");
            });

            modelBuilder.Entity<CargoInteresse>(entity =>
            {
                entity.HasOne(d => d.IdCandidatoNavigation)
                    .WithMany(p => p.CargoInteresseNavigation)
                    .HasForeignKey(d => d.IdCandidato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidato_CargoInteresse");

                entity.HasOne(d => d.IdCargoNavigation)
                    .WithMany(p => p.CargoInteresse)
                    .HasForeignKey(d => d.IdCargo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cargo_CargoInteresse");
            });

            modelBuilder.Entity<Documento>(entity =>
            {
                entity.Property(e => e.Extensao).IsUnicode(false);

                entity.HasOne(d => d.IdCandidatoNavigation)
                    .WithMany(p => p.Documento)
                    .HasForeignKey(d => d.IdCandidato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidato_Documento");

                entity.HasOne(d => d.IdEnumTipoDocumentoNavigation)
                    .WithMany(p => p.Documento)
                    .HasForeignKey(d => d.IdEnumTipoDocumento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnumTipoDocumento_Documento");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.Property(e => e.CEP).IsUnicode(false);

                entity.Property(e => e.CNPJ).IsUnicode(false);

                entity.Property(e => e.Cidade).IsUnicode(false);

                entity.Property(e => e.Endereco).IsUnicode(false);

                entity.Property(e => e.Estado).IsUnicode(false);

                entity.Property(e => e.NomeFantasia).IsUnicode(false);

                entity.Property(e => e.RazaoSocial).IsUnicode(false);
            });

            modelBuilder.Entity<EnumAgrupamento>(entity =>
            {
                entity.Property(e => e.NomeAgrupamneto).IsUnicode(false);
            });

            modelBuilder.Entity<EnumTipoDocumento>(entity =>
            {
                entity.Property(e => e.NomeAgrupamneto).IsUnicode(false);
            });

            modelBuilder.Entity<Notificacao>(entity =>
            {
                entity.Property(e => e.AngularRoute).IsUnicode(false);

                entity.Property(e => e.CorpoNotificacao).IsUnicode(false);

                entity.Property(e => e.TituloNotificacao).IsUnicode(false);

                entity.HasOne(d => d.IdCandidatoNavigation)
                    .WithMany(p => p.Notificacao)
                    .HasForeignKey(d => d.IdCandidato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidato_Notificacao");
            });

            modelBuilder.Entity<ProcessoSeletivo>(entity =>
            {
                entity.Property(e => e.NomeProcesso).IsUnicode(false);

                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.ProcessoSeletivo)
                    .HasForeignKey(d => d.IdEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Empresa_ProcessoSeletivo");
            });

            modelBuilder.Entity<Telefone>(entity =>
            {
                entity.Property(e => e.Numero).IsUnicode(false);
            });

            modelBuilder.Entity<TelefoneCandidato>(entity =>
            {
                entity.HasOne(d => d.IdCandidatoNavigation)
                    .WithMany(p => p.TelefoneCandidato)
                    .HasForeignKey(d => d.IdCandidato)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidato_TelefoneCandidato");

                entity.HasOne(d => d.IdTelefoneNavigation)
                    .WithMany(p => p.TelefoneCandidato)
                    .HasForeignKey(d => d.IdTelefone)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Telefone_TelefoneCandidato");
            });

            modelBuilder.Entity<TelefoneEmpresa>(entity =>
            {
                entity.HasOne(d => d.IdEmpresaNavigation)
                    .WithMany(p => p.TelefoneEmpresa)
                    .HasForeignKey(d => d.IdEmpresa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Empresa_TelefoneEmpresa");

                entity.HasOne(d => d.IdTelefoneNavigation)
                    .WithMany(p => p.TelefoneEmpresa)
                    .HasForeignKey(d => d.IdTelefone)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Telefone_TelefoneEmpresa");
            });

            modelBuilder.Entity<Vaga>(entity =>
            {
                entity.Property(e => e.Beneficios).IsUnicode(false);

                entity.Property(e => e.CEP).IsUnicode(false);

                entity.Property(e => e.Cidade).IsUnicode(false);

                entity.Property(e => e.Endereco).IsUnicode(false);

                entity.Property(e => e.Estado).IsUnicode(false);

                entity.Property(e => e.Latitude).IsUnicode(false);

                entity.Property(e => e.Longitude).IsUnicode(false);

                entity.Property(e => e.Requisitos).IsUnicode(false);

                entity.HasOne(d => d.IdCargoNavigation)
                    .WithMany(p => p.Vaga)
                    .HasForeignKey(d => d.IdCargo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cargo_Vaga");

                entity.HasOne(d => d.IdProcessoSeletivoNavigation)
                    .WithMany(p => p.Vaga)
                    .HasForeignKey(d => d.IdProcessoSeletivo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcessoSeletivo_Vaga");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
