using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Candidatura
    {
        [Key]
        public int Id { get; set; }
        public int IdCandidato { get; set; }
        public int IdVaga { get; set; }
        public int DataInscricao { get; set; }
        public int StatusCandidatura { get; set; }

        [ForeignKey(nameof(IdCandidato))]
        [InverseProperty(nameof(Candidato.Candidatura))]
        public virtual Candidato IdCandidatoNavigation { get; set; }
        [ForeignKey(nameof(IdVaga))]
        [InverseProperty(nameof(Vaga.Candidatura))]
        public virtual Vaga IdVagaNavigation { get; set; }
    }
}
