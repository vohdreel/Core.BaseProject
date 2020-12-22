using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Vaga
    {
        public Vaga()
        {
            Candidatura = new HashSet<Candidatura>();
        }

        [Key]
        public int Id { get; set; }
        public int IdCargo { get; set; }
        public int IdProcessoSeletivo { get; set; }
        public int Modalidade { get; set; }
        public string Requisitos { get; set; }
        public string Beneficios { get; set; }
        [Required]
        [StringLength(200)]
        public string Endereco { get; set; }
        [Required]
        [StringLength(20)]
        public string CEP { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int DisponibilidadeViagem { get; set; }
        public int DisponibilidadeTransferencia { get; set; }
        public int Jornada { get; set; }
        [Required]
        [StringLength(100)]
        public string Cidade { get; set; }
        [Required]
        [StringLength(2)]
        public string Estado { get; set; }
        [Column(TypeName = "money")]
        public decimal? Salário { get; set; }
        public int StatusVaga { get; set; }

        [ForeignKey(nameof(IdCargo))]
        [InverseProperty(nameof(Cargo.Vaga))]
        public virtual Cargo IdCargoNavigation { get; set; }
        [ForeignKey(nameof(IdProcessoSeletivo))]
        [InverseProperty(nameof(ProcessoSeletivo.Vaga))]
        public virtual ProcessoSeletivo IdProcessoSeletivoNavigation { get; set; }
        [InverseProperty("IdVagaNavigation")]
        public virtual ICollection<Candidatura> Candidatura { get; set; }
    }
}
