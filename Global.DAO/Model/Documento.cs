using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Documento
    {
        [Key]
        public int Id { get; set; }
        public int IdEnumTipoDocumento { get; set; }
        public int IdCandidato { get; set; }
        [Required]
        public byte[] ByteArray { get; set; }
        [Required]
        [StringLength(100)]
        public string Extensao { get; set; }
        [Required]
        [StringLength(500)]
        public string NomeArquivo { get; set; }
        public string Base64Code { get; set; }

        [ForeignKey(nameof(IdCandidato))]
        [InverseProperty(nameof(Candidato.Documento))]
        public virtual Candidato IdCandidatoNavigation { get; set; }
        [ForeignKey(nameof(IdEnumTipoDocumento))]
        [InverseProperty(nameof(EnumTipoDocumento.Documento))]
        public virtual EnumTipoDocumento IdEnumTipoDocumentoNavigation { get; set; }
    }
}
