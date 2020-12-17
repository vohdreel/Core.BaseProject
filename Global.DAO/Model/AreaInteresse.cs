using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class AreaInteresse
    {
        [Key]
        public int Id { get; set; }
        public int IdCandidato { get; set; }
        public int IdEnumAgrupamento { get; set; }

        [ForeignKey(nameof(IdCandidato))]
        [InverseProperty(nameof(Candidato.AreaInteresse))]
        public virtual Candidato IdCandidatoNavigation { get; set; }
        [ForeignKey(nameof(IdEnumAgrupamento))]
        [InverseProperty(nameof(EnumAgrupamento.AreaInteresse))]
        public virtual EnumAgrupamento IdEnumAgrupamentoNavigation { get; set; }
    }
}
