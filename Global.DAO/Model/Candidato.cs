using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Candidato
    {
        public Candidato()
        {
            AreaInteresse = new HashSet<AreaInteresse>();
            Candidatura = new HashSet<Candidatura>();
            CargoInteresseNavigation = new HashSet<CargoInteresse>();
            Documento = new HashSet<Documento>();
            Notificacao = new HashSet<Notificacao>();
            TelefoneCandidato = new HashSet<TelefoneCandidato>();
        }

        [Key]
        public int Id { get; set; }
        public string IDLegado { get; set; }
        [Required]
        [StringLength(11)]
        public string CPF { get; set; }
        [Required]
        [StringLength(250)]
        public string Nome { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DataNascimento { get; set; }
        [StringLength(50)]
        public string Nacionalidade { get; set; }
        [StringLength(50)]
        public string EstadoNascimento { get; set; }
        [StringLength(50)]
        public string Sexo { get; set; }
        [StringLength(50)]
        public string EstadoCivil { get; set; }
        [StringLength(50)]
        public string Raca { get; set; }
        public bool? PossuiDependentes { get; set; }
        public int? QuantidadeDependentes { get; set; }
        public bool? PossuiCNH { get; set; }
        [StringLength(2)]
        public string CategoriaCNH { get; set; }
        [StringLength(50)]
        public string CEP { get; set; }
        [StringLength(50)]
        public string Pais { get; set; }
        [StringLength(50)]
        public string Estado { get; set; }
        [StringLength(50)]
        public string Cidade { get; set; }
        [StringLength(250)]
        public string Endereco { get; set; }
        [StringLength(250)]
        public string NumeroEcomplemetno { get; set; }
        [StringLength(50)]
        public string Bairro { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string EmailSecundario { get; set; }
        [StringLength(50)]
        public string Identidade { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DataEmissao { get; set; }
        [StringLength(50)]
        public string OrgaoEmissor { get; set; }
        [StringLength(50)]
        public string IdentidadePais { get; set; }
        [StringLength(50)]
        public string IdentidadeEstado { get; set; }
        [StringLength(250)]
        public string NomeMae { get; set; }
        [StringLength(50)]
        public string NomePai { get; set; }
        public bool? Deficiente { get; set; }
        [StringLength(50)]
        public string CID { get; set; }
        [StringLength(50)]
        public string AmputacaoMembrosInferiores { get; set; }
        [StringLength(50)]
        public string AmputacaoMembrosSuperiores { get; set; }
        [StringLength(50)]
        public string DeficienciaAudicao { get; set; }
        [StringLength(50)]
        public string DeficienciasMentais { get; set; }
        [StringLength(50)]
        public string DeficienciaCrescimento { get; set; }
        [StringLength(50)]
        public string DeficienciaFala { get; set; }
        public string Observacoes { get; set; }
        [StringLength(50)]
        public string TipoVaga { get; set; }
        [StringLength(50)]
        public string LocalPreferencia { get; set; }
        [StringLength(50)]
        public string LocalPreferenciaSecundario { get; set; }
        [StringLength(50)]
        public string DisponibilidadeHorario { get; set; }
        [StringLength(50)]
        public string DisponibilidadeViagem { get; set; }
        [StringLength(50)]
        public string DisponibilidadeTransferencia { get; set; }
        [StringLength(50)]
        public string PretencaoSalarial { get; set; }
        [StringLength(100)]
        public string CargoInteresse { get; set; }
        [StringLength(100)]
        public string CagoInteresseSecundario { get; set; }
        public bool? TermoCompromisso { get; set; }
        public bool MaterConectado { get; set; }
        [Required]
        [StringLength(100)]
        public string IdAspNetUsers { get; set; }
        public string FCMToken { get; set; }

        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<AreaInteresse> AreaInteresse { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<Candidatura> Candidatura { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<CargoInteresse> CargoInteresseNavigation { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<Documento> Documento { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<Notificacao> Notificacao { get; set; }
        [InverseProperty("IdCandidatoNavigation")]
        public virtual ICollection<TelefoneCandidato> TelefoneCandidato { get; set; }
    }
}
