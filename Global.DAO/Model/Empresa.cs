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
        [StringLength(200)]
        public string RazaoSocial { get; set; }
        [StringLength(200)]
        public string Endereco { get; set; }
        [Column("CEP")]
        [StringLength(20)]
        public string Cep { get; set; }
        [Column("CNPJ")]
        [StringLength(14)]
        public string Cnpj { get; set; }
        [StringLength(100)]
        public string Cidade { get; set; }
        [StringLength(2)]
        public string Estado { get; set; }
        public string Base64ImageLogo { get; set; }
        [StringLength(50)]
        public string ImageLogoExtension { get; set; }
        [StringLength(150)]
        public string EmailContato { get; set; }

        [InverseProperty("IdEmpresaNavigation")]
        public virtual ICollection<ProcessoSeletivo> ProcessoSeletivo { get; set; }
        [InverseProperty("IdEmpresaNavigation")]
        public virtual ICollection<TelefoneEmpresa> TelefoneEmpresa { get; set; }
    }
}
