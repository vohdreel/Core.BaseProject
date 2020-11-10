using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class TipoRepase
    {
        public TipoRepase()
        {
            ContratoUnidade = new HashSet<ContratoUnidade>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        [InverseProperty("TipoRepase")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdtipoRepaceNavigation")]
        public virtual ICollection<ContratoUnidade> ContratoUnidade { get; set; }
    }
}
