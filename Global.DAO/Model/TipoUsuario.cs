using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class TipoUsuario
    {
        public TipoUsuario()
        {
            UnidadeTipoUsuario = new HashSet<UnidadeTipoUsuario>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        [InverseProperty("TipoUsuario")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdtipoUsuarioNavigation")]
        public virtual ICollection<UnidadeTipoUsuario> UnidadeTipoUsuario { get; set; }
    }
}
