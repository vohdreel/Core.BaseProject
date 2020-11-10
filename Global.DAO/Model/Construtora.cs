using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Construtora
    {
        public Construtora()
        {
            Unidade = new HashSet<Unidade>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Construtora")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdconstrutoraNavigation")]
        public virtual ICollection<Unidade> Unidade { get; set; }
    }
}
