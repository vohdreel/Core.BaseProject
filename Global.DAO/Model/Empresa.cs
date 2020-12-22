using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Empresa
    {
        public Empresa()
        {
            ProcessoSeletivo = new HashSet<ProcessoSeletivo>();
            TelefoneEmpresa = new HashSet<TelefoneEmpresa>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string NomeFantasia { get; set; }
        [Required]
        [StringLength(200)]
        public string RazaoSocial { get; set; }
        [Required]
        [StringLength(200)]
        public string Endereco { get; set; }
        [Required]
        [StringLength(20)]
        public string CEP { get; set; }
        [Required]
        [StringLength(14)]
        public string CNPJ { get; set; }
        [Required]
        [StringLength(100)]
        public string Cidade { get; set; }
        [Required]
        [StringLength(2)]
        public string Estado { get; set; }

        [InverseProperty("IdEmpresaNavigation")]
        public virtual ICollection<ProcessoSeletivo> ProcessoSeletivo { get; set; }
        [InverseProperty("IdEmpresaNavigation")]
        public virtual ICollection<TelefoneEmpresa> TelefoneEmpresa { get; set; }
    }
}
