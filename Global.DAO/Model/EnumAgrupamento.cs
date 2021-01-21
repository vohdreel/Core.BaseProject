using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class EnumAgrupamento
    {
        public EnumAgrupamento()
        {
            AreaInteresse = new HashSet<AreaInteresse>();
            Cargo = new HashSet<Cargo>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string NomeAgrupamento { get; set; }

        [InverseProperty("IdEnumAgrupamentoNavigation")]
        public virtual ICollection<AreaInteresse> AreaInteresse { get; set; }
        [InverseProperty("IdEnumAgrupamentoNavigation")]
        public virtual ICollection<Cargo> Cargo { get; set; }
    }
}
