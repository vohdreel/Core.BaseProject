using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class TipoEmpreendimento
    {
        public TipoEmpreendimento()
        {
            Unidade = new HashSet<Unidade>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        [InverseProperty("TipoEmpreendimento")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdtipoEmpreendimentoNavigation")]
        public virtual ICollection<Unidade> Unidade { get; set; }
    }
}
