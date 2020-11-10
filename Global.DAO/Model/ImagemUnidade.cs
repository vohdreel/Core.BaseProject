using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class ImagemUnidade
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string NomeArquivo { get; set; }
        [Column("IDFile")]
        public Guid Idfile { get; set; }
        [Column("IDUnidade")]
        public Guid Idunidade { get; set; }
        [Column("StatusID")]
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(Idunidade))]
        [InverseProperty(nameof(Unidade.ImagemUnidade))]
        public virtual Unidade IdunidadeNavigation { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("ImagemUnidade")]
        public virtual Status Status { get; set; }
    }
}
