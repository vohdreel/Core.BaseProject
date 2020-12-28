using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Notificacao
    {
        [Key]
        public int Id { get; set; }
        public int IdCandidato { get; set; }
        [Required]
        [StringLength(60)]
        public string TituloNotificacao { get; set; }
        [Required]
        [StringLength(200)]
        public string CorpoNotificacao { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DataCriacaoNotificacao { get; set; }
        [Required]
        [StringLength(60)]
        public string AngularRoute { get; set; }
        public bool Visualizado { get; set; }

        [ForeignKey(nameof(IdCandidato))]
        [InverseProperty(nameof(Candidato.Notificacao))]
        public virtual Candidato IdCandidatoNavigation { get; set; }
    }
}
