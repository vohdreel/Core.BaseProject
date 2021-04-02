using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Telefone
    {
        public Telefone()
        {
            TelefoneCandidato = new HashSet<TelefoneCandidato>();
            TelefoneEmpresa = new HashSet<TelefoneEmpresa>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Numero { get; set; }
        public int? TipoTelefone { get; set; }

        [InverseProperty("IdTelefoneNavigation")]
        public virtual ICollection<TelefoneCandidato> TelefoneCandidato { get; set; }
        [InverseProperty("IdTelefoneNavigation")]
        public virtual ICollection<TelefoneEmpresa> TelefoneEmpresa { get; set; }
    }
}
