using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class CargoInteresse
    {
        [Key]
        public int Id { get; set; }
        public int IdCandidato { get; set; }
        public int IdCargo { get; set; }

        [ForeignKey(nameof(IdCandidato))]
        [InverseProperty(nameof(Candidato.CargoInteresseNavigation))]
        public virtual Candidato IdCandidatoNavigation { get; set; }
        [ForeignKey(nameof(IdCargo))]
        [InverseProperty(nameof(Cargo.CargoInteresse))]
        public virtual Cargo IdCargoNavigation { get; set; }
    }
}
