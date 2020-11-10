using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class ContratoUnidade
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PrazoContrato { get; set; }
        [Column(TypeName = "money")]
        public decimal ValorAdministracao { get; set; }
        [Column(TypeName = "money")]
        public decimal ValorGarantiaMinima { get; set; }
        [Column("IDUnidade")]
        public Guid Idunidade { get; set; }
        [Column("IDTipoRepace")]
        public Guid IdtipoRepace { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(IdtipoRepace))]
        [InverseProperty(nameof(TipoRepase.ContratoUnidade))]
        public virtual TipoRepase IdtipoRepaceNavigation { get; set; }
        [ForeignKey(nameof(Idunidade))]
        [InverseProperty(nameof(Unidade.ContratoUnidade))]
        public virtual Unidade IdunidadeNavigation { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("ContratoUnidade")]
        public virtual Status Status { get; set; }
    }
}
