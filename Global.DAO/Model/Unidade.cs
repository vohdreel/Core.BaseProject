using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Unidade
    {
        public Unidade()
        {
            CargoUnidade = new HashSet<CargoUnidade>();
            ContatosUnidade = new HashSet<ContatosUnidade>();
            ContratoUnidade = new HashSet<ContratoUnidade>();
            FaixaFaturamento = new HashSet<FaixaFaturamento>();
            ImagemUnidade = new HashSet<ImagemUnidade>();
            OrcamentoUnidade = new HashSet<OrcamentoUnidade>();
            PavimentoUnidade = new HashSet<PavimentoUnidade>();
            PeriodoUnidade = new HashSet<PeriodoUnidade>();
            UnidadeTipoUsuario = new HashSet<UnidadeTipoUsuario>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string NomeUnidade { get; set; }
        [Column("CEPUnidade")]
        [StringLength(100)]
        public string Cepunidade { get; set; }
        [StringLength(200)]
        public string LogradouroUnidade { get; set; }
        public int? NumeroLogradouroUnidade { get; set; }
        [StringLength(200)]
        public string ComplementoLogradouroUnidade { get; set; }
        [StringLength(200)]
        public string BairroUnidade { get; set; }
        [Column("IDCidade")]
        public Guid? Idcidade { get; set; }
        [Column("IDEstado")]
        public Guid? Idestado { get; set; }
        [Column("IDPais")]
        public Guid? Idpais { get; set; }
        [Column("IDAdministradora")]
        public Guid? Idadministradora { get; set; }
        [Column("IDConstrutora")]
        public Guid? Idconstrutora { get; set; }
        [Column("IDTipoEmpreendimento")]
        public Guid? IdtipoEmpreendimento { get; set; }
        public int? TotalVagasFisicas { get; set; }
        public bool? Flag24Horas { get; set; }
        public bool? FlagSistemaGaragem { get; set; }
        public bool? FlagCancela { get; set; }
        public bool? FlagTerceiro { get; set; }
        public bool? FlagAutomacaoTerceiro { get; set; }
        public bool? FlagParticipaRateio { get; set; }
        public Guid StatusUnidadeId { get; set; }

        [ForeignKey(nameof(Idadministradora))]
        [InverseProperty(nameof(Administradora.Unidade))]
        public virtual Administradora IdadministradoraNavigation { get; set; }
        [ForeignKey(nameof(Idcidade))]
        [InverseProperty(nameof(Cidade.Unidade))]
        public virtual Cidade IdcidadeNavigation { get; set; }
        [ForeignKey(nameof(Idconstrutora))]
        [InverseProperty(nameof(Construtora.Unidade))]
        public virtual Construtora IdconstrutoraNavigation { get; set; }
        [ForeignKey(nameof(Idestado))]
        [InverseProperty(nameof(Estado.Unidade))]
        public virtual Estado IdestadoNavigation { get; set; }
        [ForeignKey(nameof(Idpais))]
        [InverseProperty(nameof(Pais.Unidade))]
        public virtual Pais IdpaisNavigation { get; set; }
        [ForeignKey(nameof(IdtipoEmpreendimento))]
        [InverseProperty(nameof(TipoEmpreendimento.Unidade))]
        public virtual TipoEmpreendimento IdtipoEmpreendimentoNavigation { get; set; }
        [ForeignKey(nameof(StatusUnidadeId))]
        [InverseProperty("Unidade")]
        public virtual StatusUnidade StatusUnidade { get; set; }
        [InverseProperty("IdunidadeNavigation")]
        public virtual ICollection<CargoUnidade> CargoUnidade { get; set; }
        [InverseProperty("IdunidadeNavigation")]
        public virtual ICollection<ContatosUnidade> ContatosUnidade { get; set; }
        [InverseProperty("IdunidadeNavigation")]
        public virtual ICollection<ContratoUnidade> ContratoUnidade { get; set; }
        [InverseProperty("IdunidadeNavigation")]
        public virtual ICollection<FaixaFaturamento> FaixaFaturamento { get; set; }
        [InverseProperty("IdunidadeNavigation")]
        public virtual ICollection<ImagemUnidade> ImagemUnidade { get; set; }
        [InverseProperty("IdunidadeNavigation")]
        public virtual ICollection<OrcamentoUnidade> OrcamentoUnidade { get; set; }
        [InverseProperty("IdunidadeNavigation")]
        public virtual ICollection<PavimentoUnidade> PavimentoUnidade { get; set; }
        [InverseProperty("IdunidadeNavigation")]
        public virtual ICollection<PeriodoUnidade> PeriodoUnidade { get; set; }
        [InverseProperty("IdunidadeNavigation")]
        public virtual ICollection<UnidadeTipoUsuario> UnidadeTipoUsuario { get; set; }
    }
}
