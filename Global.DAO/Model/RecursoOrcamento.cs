using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class RecursoOrcamento
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        public int Quantidade { get; set; }
        [Column("IDOrcamentoUnidade")]
        public Guid IdorcamentoUnidade { get; set; }
        [Column("IDRercuso")]
        public Guid Idrercuso { get; set; }

        [ForeignKey(nameof(IdorcamentoUnidade))]
        [InverseProperty(nameof(OrcamentoUnidade.RecursoOrcamento))]
        public virtual OrcamentoUnidade IdorcamentoUnidadeNavigation { get; set; }
        [ForeignKey(nameof(Idrercuso))]
        [InverseProperty(nameof(Recurso.RecursoOrcamento))]
        public virtual Recurso IdrercusoNavigation { get; set; }
    }
}
