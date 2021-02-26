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
        [Column("CEP")]
        [StringLength(20)]
        public string Cep { get; set; }
        [Required]
        [Column("CNPJ")]
        [StringLength(14)]
        public string Cnpj { get; set; }
        [Required]
        [StringLength(100)]
        public string Cidade { get; set; }
        [Required]
        [StringLength(2)]
        public string Estado { get; set; }
        public string Base64ImageLogo { get; set; }
        [StringLength(50)]
        public string ImageLogoExtension { get; set; }

        [InverseProperty("IdEmpresaNavigation")]
        public virtual ICollection<ProcessoSeletivo> ProcessoSeletivo { get; set; }
        [InverseProperty("IdEmpresaNavigation")]
        public virtual ICollection<TelefoneEmpresa> TelefoneEmpresa { get; set; }
    }
}
