using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class PeriodoUnidade
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime HoraInicio { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime HoraFim { get; set; }
        [Column("IDPeriodo")]
        public Guid Idperiodo { get; set; }
        [Column("IDUnidade")]
        public Guid Idunidade { get; set; }

        [ForeignKey(nameof(Idperiodo))]
        [InverseProperty(nameof(Periodo.PeriodoUnidade))]
        public virtual Periodo IdperiodoNavigation { get; set; }
        [ForeignKey(nameof(Idunidade))]
        [InverseProperty(nameof(Unidade.PeriodoUnidade))]
        public virtual Unidade IdunidadeNavigation { get; set; }
    }
}
