using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Pais
    {
        public Pais()
        {
            Empresa = new HashSet<Empresa>();
            Estado = new HashSet<Estado>();
            Unidade = new HashSet<Unidade>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Nome { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Pais")]
        public virtual Status Status { get; set; }
        [InverseProperty("PaisDoCapital")]
        public virtual ICollection<Empresa> Empresa { get; set; }
        [InverseProperty("Pais")]
        public virtual ICollection<Estado> Estado { get; set; }
        [InverseProperty("IdpaisNavigation")]
        public virtual ICollection<Unidade> Unidade { get; set; }
    }
}
