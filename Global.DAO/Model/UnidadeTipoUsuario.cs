using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class UnidadeTipoUsuario
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Column("IDUnidade")]
        public Guid Idunidade { get; set; }
        [Column("IDTipoUsuario")]
        public Guid IdtipoUsuario { get; set; }

        [ForeignKey(nameof(IdtipoUsuario))]
        [InverseProperty(nameof(TipoUsuario.UnidadeTipoUsuario))]
        public virtual TipoUsuario IdtipoUsuarioNavigation { get; set; }
        [ForeignKey(nameof(Idunidade))]
        [InverseProperty(nameof(Unidade.UnidadeTipoUsuario))]
        public virtual Unidade IdunidadeNavigation { get; set; }
    }
}
