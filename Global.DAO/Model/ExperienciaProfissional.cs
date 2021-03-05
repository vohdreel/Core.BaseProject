using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class ExperienciaProfissional
    {
        [Key]
        public int Id { get; set; }
        public int IdCandidato { get; set; }
        [StringLength(250)]
        public string Empresa { get; set; }
        [StringLength(250)]
        public string Cargo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DataAdmissão { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DataDesligamento { get; set; }

        [ForeignKey(nameof(IdCandidato))]
        [InverseProperty(nameof(Candidato.ExperienciaProfissional))]
        public virtual Candidato IdCandidatoNavigation { get; set; }
    }
}
