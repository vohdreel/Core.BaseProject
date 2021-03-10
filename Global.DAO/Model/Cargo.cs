using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Cargo
    {
        public Cargo()
        {
            CargoInteresse = new HashSet<CargoInteresse>();
            Vaga = new HashSet<Vaga>();
        }

        [Key]
        public int Id { get; set; }
        public int IdEnumAgrupamento { get; set; }
        [Required]
        [StringLength(100)]
        public string NomeCargo { get; set; }
        [StringLength(250)]
        public string DescricaoCargo { get; set; }
        public int? ReferenceNumber { get; set; }

        [ForeignKey(nameof(IdEnumAgrupamento))]
        [InverseProperty(nameof(EnumAgrupamento.Cargo))]
        public virtual EnumAgrupamento IdEnumAgrupamentoNavigation { get; set; }
        [InverseProperty("IdCargoNavigation")]
        public virtual ICollection<CargoInteresse> CargoInteresse { get; set; }
        [InverseProperty("IdCargoNavigation")]
        public virtual ICollection<Vaga> Vaga { get; set; }
    }
}
