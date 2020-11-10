using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Estado
    {
        public Estado()
        {
            Cidade = new HashSet<Cidade>();
            Unidade = new HashSet<Unidade>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(2)]
        public string Sigla { get; set; }
        [Required]
        [StringLength(128)]
        public string Nome { get; set; }
        public Guid PaisId { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(PaisId))]
        [InverseProperty("Estado")]
        public virtual Pais Pais { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Estado")]
        public virtual Status Status { get; set; }
        [InverseProperty("Estado")]
        public virtual ICollection<Cidade> Cidade { get; set; }
        [InverseProperty("IdestadoNavigation")]
        public virtual ICollection<Unidade> Unidade { get; set; }
    }
}
