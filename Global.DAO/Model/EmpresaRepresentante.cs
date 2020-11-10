using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class EmpresaRepresentante
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        public Guid EmpresaId { get; set; }

        [ForeignKey(nameof(EmpresaId))]
        [InverseProperty("EmpresaRepresentante")]
        public virtual Empresa Empresa { get; set; }
    }
}
