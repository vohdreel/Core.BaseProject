using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class VagaFavorita
    {
        [Key]
        public int Id { get; set; }
        public int IdCandidato { get; set; }
        public int IdVaga { get; set; }

        [ForeignKey(nameof(IdCandidato))]
        [InverseProperty(nameof(Candidato.VagaFavorita))]
        public virtual Candidato IdCandidatoNavigation { get; set; }
        [ForeignKey(nameof(IdVaga))]
        [InverseProperty(nameof(Vaga.VagaFavorita))]
        public virtual Vaga IdVagaNavigation { get; set; }
    }
}
