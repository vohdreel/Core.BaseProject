using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class TipoRecurso
    {
        public TipoRecurso()
        {
            Recurso = new HashSet<Recurso>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        public Guid? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        [InverseProperty("TipoRecurso")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdtipoRecursoNavigation")]
        public virtual ICollection<Recurso> Recurso { get; set; }
    }
}
