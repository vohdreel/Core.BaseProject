using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class FaixaFaturamento
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Column(TypeName = "money")]
        public decimal ValorInicial { get; set; }
        [Column(TypeName = "money")]
        public decimal ValorFinal { get; set; }
        public double Porcentagem { get; set; }
        [Column("IDUnidade")]
        public Guid Idunidade { get; set; }
        [Column("StatusID")]
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(Idunidade))]
        [InverseProperty(nameof(Unidade.FaixaFaturamento))]
        public virtual Unidade IdunidadeNavigation { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("FaixaFaturamento")]
        public virtual Status Status { get; set; }
    }
}
