using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class EnumTipoDocumento
    {
        public EnumTipoDocumento()
        {
            Documento = new HashSet<Documento>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string NomeTipoDocumento { get; set; }

        [InverseProperty("IdEnumTipoDocumentoNavigation")]
        public virtual ICollection<Documento> Documento { get; set; }
    }
}
