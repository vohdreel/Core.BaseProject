using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class ContatosUnidade
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string NomeContato { get; set; }
        [Required]
        [StringLength(200)]
        public string TelefoneContato { get; set; }
        [Required]
        [StringLength(200)]
        public string EmailContato { get; set; }
        [Column("IDUnidade")]
        public Guid Idunidade { get; set; }
        [Column("StatusID")]
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(Idunidade))]
        [InverseProperty(nameof(Unidade.ContatosUnidade))]
        public virtual Unidade IdunidadeNavigation { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("ContatosUnidade")]
        public virtual Status Status { get; set; }
    }
}
