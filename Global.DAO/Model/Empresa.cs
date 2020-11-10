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
            ContatoEmpresa = new HashSet<ContatoEmpresa>();
            EmpresaRepresentante = new HashSet<EmpresaRepresentante>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        public Guid CidadeId { get; set; }
        [Required]
        [StringLength(128)]
        public string NomeFantasia { get; set; }
        [Required]
        [StringLength(128)]
        public string RazaoSocial { get; set; }
        [Column("CNPJ")]
        [StringLength(18)]
        public string Cnpj { get; set; }
        [Column("CEP")]
        [StringLength(9)]
        public string Cep { get; set; }
        [Required]
        [StringLength(128)]
        public string Rua { get; set; }
        [Required]
        [StringLength(32)]
        public string Numero { get; set; }
        [StringLength(64)]
        public string Complemento { get; set; }
        [Required]
        [StringLength(64)]
        public string Bairro { get; set; }
        [StringLength(14)]
        public string Telefone { get; set; }
        public Guid? PaisDoCapitalId { get; set; }
        public int? NumeroDeFuncionarios { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(CidadeId))]
        [InverseProperty("Empresa")]
        public virtual Cidade Cidade { get; set; }
        [ForeignKey(nameof(PaisDoCapitalId))]
        [InverseProperty(nameof(Pais.Empresa))]
        public virtual Pais PaisDoCapital { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("Empresa")]
        public virtual Status Status { get; set; }
        [InverseProperty("Empresa")]
        public virtual ICollection<ContatoEmpresa> ContatoEmpresa { get; set; }
        [InverseProperty("Empresa")]
        public virtual ICollection<EmpresaRepresentante> EmpresaRepresentante { get; set; }
    }
}
