using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Status
    {
        public Status()
        {
            AcessoPavimento = new HashSet<AcessoPavimento>();
            Administradora = new HashSet<Administradora>();
            CargoUnidade = new HashSet<CargoUnidade>();
            Cidade = new HashSet<Cidade>();
            Construtora = new HashSet<Construtora>();
            ContatoEmpresa = new HashSet<ContatoEmpresa>();
            ContatosUnidade = new HashSet<ContatosUnidade>();
            ContratoUnidade = new HashSet<ContratoUnidade>();
            Empresa = new HashSet<Empresa>();
            Estado = new HashSet<Estado>();
            FaixaFaturamento = new HashSet<FaixaFaturamento>();
            ImagemUnidade = new HashSet<ImagemUnidade>();
            OpcaoAcesso = new HashSet<OpcaoAcesso>();
            OrcamentoUnidade = new HashSet<OrcamentoUnidade>();
            Pais = new HashSet<Pais>();
            PavimentoUnidade = new HashSet<PavimentoUnidade>();
            Periodo = new HashSet<Periodo>();
            Recurso = new HashSet<Recurso>();
            StatusUnidade = new HashSet<StatusUnidade>();
            TipoEmpreendimento = new HashSet<TipoEmpreendimento>();
            TipoRecurso = new HashSet<TipoRecurso>();
            TipoRepase = new HashSet<TipoRepase>();
            TipoUsuario = new HashSet<TipoUsuario>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [InverseProperty("Status")]
        public virtual ICollection<AcessoPavimento> AcessoPavimento { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<Administradora> Administradora { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<CargoUnidade> CargoUnidade { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<Cidade> Cidade { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<Construtora> Construtora { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<ContatoEmpresa> ContatoEmpresa { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<ContatosUnidade> ContatosUnidade { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<ContratoUnidade> ContratoUnidade { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<Empresa> Empresa { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<Estado> Estado { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<FaixaFaturamento> FaixaFaturamento { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<ImagemUnidade> ImagemUnidade { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<OpcaoAcesso> OpcaoAcesso { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<OrcamentoUnidade> OrcamentoUnidade { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<Pais> Pais { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<PavimentoUnidade> PavimentoUnidade { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<Periodo> Periodo { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<Recurso> Recurso { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<StatusUnidade> StatusUnidade { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<TipoEmpreendimento> TipoEmpreendimento { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<TipoRecurso> TipoRecurso { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<TipoRepase> TipoRepase { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<TipoUsuario> TipoUsuario { get; set; }
    }
}
