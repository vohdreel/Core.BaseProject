using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Cidade
    {
        public Cidade()
        {
            Empresa = new HashSet<Empresa>();
            Unidade = new HashSet<Unidade>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        public Guid EstadoId { get; set; }
        [Required]
        [StringLength(128)]
        public string Nome { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(EstadoId))]
        [InverseProperty("Cidade")]
        public virtual Estado Estado { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Cidade")]
        public virtual Status Status { get; set; }
        [InverseProperty("Cidade")]
        public virtual ICollection<Empresa> Empresa { get; set; }
        [InverseProperty("IdcidadeNavigation")]
        public virtual ICollection<Unidade> Unidade { get; set; }
    }
}
