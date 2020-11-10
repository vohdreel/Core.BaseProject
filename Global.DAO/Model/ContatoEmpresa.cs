using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class ContatoEmpresa
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid UserId { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(EmpresaId))]
        [InverseProperty("ContatoEmpresa")]
        public virtual Empresa Empresa { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("ContatoEmpresa")]
        public virtual Status Status { get; set; }
    }
}
