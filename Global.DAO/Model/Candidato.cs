using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Candidato
    {
        public Candidato()
        {
            AreaInteresse = new HashSet<AreaInteresse>();
            Candidatura = new HashSet<Candidatura>();
            CargoInteresseNavigation = new HashSet<CargoInteresse>();
            Documento = new HashSet<Documento>();
            ExperienciaProfissional = new HashSet<ExperienciaProfissional>();
            FormacaoCandidato = new HashSet<FormacaoCandidato>();
            Notificacao = new HashSet<Notificacao>();
            TelefoneCandidato = new HashSet<TelefoneCandidato>();
            VagaFavorita = new HashSet<VagaFavorita>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("IDLegado")]
        public string Idlegado { get; set; }
        [Required]
        [Column("CPF")]
        [StringLength(11)]
        public string Cpf { get; set; }
        [Required]
        [StringLength(250)]
        public string Nome { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DataNascimento { get; set; }
        [StringLength(50)]
        public string Nacionalidade { get; set; }
        [StringLength(50)]
        public string EstadoNascimento { get; set; }
        [StringLength(50)]
        public string Sexo { get; set; }
        public int? EstadoCivil { get; set; }
        public int? Raca { get; set; }
        public bool? PossuiDependentes { get; set; }
        public int? QuantidadeDependentes { get; set; }
        [Column("PossuiCNH")]
        public bool? PossuiCnh { get; set; }
        [Column("CategoriaCNH")]
        [StringLength(2)]
        public string CategoriaCnh { get; set; }
        [Column("CEP")]
        [StringLength(50)]
        public string Cep { get; set; }
        [StringLength(300)]
        public string Pais { get; set; }
        [StringLength(50)]
        public string Estado { get; set; }
        [StringLength(300)]
        public string Cidade { get; set; }
        [StringLength(250)]
        public string Endereco { get; set; }
        [StringLength(250)]
        public string NumeroEcomplemetno { get; set; }
        [StringLength(300)]
        public string Bairro { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string EmailSecundario { get; set; }
        [StringLength(50)]
        public string Identidade { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DataEmissao { get; set; }
        [StringLength(50)]
        public string OrgaoEmissor { get; set; }
        [StringLength(50)]
        public string IdentidadePais { get; set; }
        [StringLength(50)]
        public string IdentidadeEstado { get; set; }
        [StringLength(250)]
        public string NomeMae { get; set; }
        [StringLength(50)]
        public string NomePai { get; set; }
        public bool? Deficiente { get; set; }
        [Column("CID")]
        [StringLength(50)]
        public string Cid { get; set; }
        [StringLength(50)]
        public string AmputacaoMembrosInferiores { get; set; }
        [StringLength(50)]
        public string AmputacaoMembrosSuperiores { get; set; }
        [StringLength(50)]
        public string DeficienciaAudicao { get; set; }
        [StringLength(50)]
        public string DeficienciasMentais { get; set; }
        [StringLength(50)]
        public string DeficienciaCrescimento { get; set; }
        [StringLength(50)]
        public string DeficienciaFala { get; set; }
        public string Observacoes { get; set; }
        [StringLength(50)]
        public string TipoVaga { get; set; }
        [StringLength(50)]
        public string LocalPreferencia { get; set; }
        [StringLength(50)]
        public string LocalPreferenciaSecundario { get; set; }
        public int? DisponibilidadeHorario { get; set; }
        public int? DisponibilidadeViagem { get; set; }
        public int? DisponibilidadeTransferencia { get; set; }
        public int? PretensaoSalarial { get; set; }
        public bool? TermoCompromisso { get; set; }
        public bool MaterConectado { get; set; }
        [StringLength(100)]
        public string IdAspNetUsers { get; set; }
        [Column("FCMToken")]
        public string Fcmtoken { get; set; }
        [StringLength(100)]
        public string Latitude { get; set; }
        [StringLength(100)]
        public string Longitude { get; set; }
        [StringLength(20)]
        public string Numero { get; set; }
        [StringLength(300)]
        public string Complemento { get; set; }
        [StringLength(200)]
        public string Agrupadores { get; set; }
        public int? PerfilProfissional { get; set; }
        public int? NivelProfissionalVagaDesejada { get; set; }
        public int? Idade { get; set; }
        public string FotoCandidato { get; set; }
        [StringLength(150)]
        public string TesteOnlineCandidato { get; set; }
        [StringLength(150)]
        public string IntervaloData { get; set; }
        [StringLength(150)]
        public string NomeProcesso { get; set; }
        [StringLength(150)]
        public string SituacaoPlanoSaude { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DataSituacaoPlanoSaude { get; set; }
        [StringLength(150)]
        public string EmpresaAtual { get; set; }
        [StringLength(150)]
        public string CargoAtual { get; set; }
        [StringLength(150)]
        public string AreaAtuacao { get; set; }
        public int? NiveIngles { get; set; }
        public int? NiveEspanhol { get; set; }
        [StringLength(150)]
        public string MatematicaFinanceira { get; set; }
        [StringLength(150)]
        public string CargoInteresse { get; set; }
        [StringLength(150)]
        public string CargoInteresseSecundario { get; set; }
        public bool? PrimeiroEmprego { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DataInscricao { get; set; }

        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<AreaInteresse> AreaInteresse { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<Candidatura> Candidatura { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<CargoInteresse> CargoInteresseNavigation { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<Documento> Documento { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<ExperienciaProfissional> ExperienciaProfissional { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<FormacaoCandidato> FormacaoCandidato { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<Notificacao> Notificacao { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<TelefoneCandidato> TelefoneCandidato { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<VagaFavorita> VagaFavorita { get; set; }
    }
}
