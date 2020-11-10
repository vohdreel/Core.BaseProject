using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Recurso
    {
        public Recurso()
        {
            RecursoOrcamento = new HashSet<RecursoOrcamento>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        [Column(TypeName = "money")]
        public decimal Valor { get; set; }
        [Column("IDTipoRecurso")]
        public Guid IdtipoRecurso { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(IdtipoRecurso))]
        [InverseProperty(nameof(TipoRecurso.Recurso))]
        public virtual TipoRecurso IdtipoRecursoNavigation { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Recurso")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdrercusoNavigation")]
        public virtual ICollection<RecursoOrcamento> RecursoOrcamento { get; set; }
    }
}
