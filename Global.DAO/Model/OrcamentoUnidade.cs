using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class OrcamentoUnidade
    {
        public OrcamentoUnidade()
        {
            RecursoOrcamento = new HashSet<RecursoOrcamento>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        public int? QuantidadeMensalista { get; set; }
        public int? QuantidadeAvulso { get; set; }
        public int? QuantidadeConvenio { get; set; }
        [Column(TypeName = "money")]
        public decimal? ValorMensalista { get; set; }
        [Column(TypeName = "money")]
        public decimal? ValorAvulso { get; set; }
        [Column(TypeName = "money")]
        public decimal? ValorConvenio { get; set; }
        [Column(TypeName = "money")]
        public decimal? CenarioPessimo { get; set; }
        [Column(TypeName = "money")]
        public decimal? CenarioOtimista { get; set; }
        [Column("IDUnidade")]
        public Guid? Idunidade { get; set; }
        public Guid? StatusId { get; set; }

        [ForeignKey(nameof(Idunidade))]
        [InverseProperty(nameof(Unidade.OrcamentoUnidade))]
        public virtual Unidade IdunidadeNavigation { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("OrcamentoUnidade")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdorcamentoUnidadeNavigation")]
        public virtual ICollection<RecursoOrcamento> RecursoOrcamento { get; set; }
    }
}
