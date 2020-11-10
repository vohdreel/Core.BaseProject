using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Periodo
    {
        public Periodo()
        {
            PeriodoUnidade = new HashSet<PeriodoUnidade>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Periodo")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdperiodoNavigation")]
        public virtual ICollection<PeriodoUnidade> PeriodoUnidade { get; set; }
    }
}
