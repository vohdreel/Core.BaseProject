using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class TelefoneCandidato
    {
        [Key]
        public int Id { get; set; }
        public int IdTelefone { get; set; }
        public int IdCandidato { get; set; }

        [ForeignKey(nameof(IdCandidato))]
        [InverseProperty(nameof(Candidato.TelefoneCandidato))]
        public virtual Candidato IdCandidatoNavigation { get; set; }
        [ForeignKey(nameof(IdTelefone))]
        [InverseProperty(nameof(Telefone.TelefoneCandidato))]
        public virtual Telefone IdTelefoneNavigation { get; set; }
    }
}
