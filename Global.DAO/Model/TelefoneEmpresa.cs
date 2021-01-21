using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class TelefoneEmpresa
    {
        [Key]
        public int Id { get; set; }
        public int IdTelefone { get; set; }
        public int IdEmpresa { get; set; }

        [ForeignKey(nameof(IdEmpresa))]
        [InverseProperty(nameof(Empresa.TelefoneEmpresa))]
        public virtual Empresa IdEmpresaNavigation { get; set; }
        [ForeignKey(nameof(IdTelefone))]
        [InverseProperty(nameof(Telefone.TelefoneEmpresa))]
        public virtual Telefone IdTelefoneNavigation { get; set; }
    }
}
