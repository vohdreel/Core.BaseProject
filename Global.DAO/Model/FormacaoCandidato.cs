using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class FormacaoCandidato
    {
        [Key]
        public int Id { get; set; }
        public int IdCandidato { get; set; }
        public int? TipoFormacao { get; set; }
        public int? Modalidade { get; set; }
        [StringLength(250)]
        public string Instituicao { get; set; }
        [StringLength(250)]
        public string Curso { get; set; }
        public int? Situacao { get; set; }
        [StringLength(250)]
        public string DataConclusao { get; set; }

        [ForeignKey(nameof(IdCandidato))]
        [InverseProperty(nameof(Candidato.FormacaoCandidato))]
        public virtual Candidato IdCandidatoNavigation { get; set; }
    }
}
