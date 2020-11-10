using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class PavimentoUnidade
    {
        public PavimentoUnidade()
        {
            AcessoPavimento = new HashSet<AcessoPavimento>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        [Column("IDUnidade")]
        public Guid Idunidade { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(Idunidade))]
        [InverseProperty(nameof(Unidade.PavimentoUnidade))]
        public virtual Unidade IdunidadeNavigation { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("PavimentoUnidade")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdpavimentoUnidadeNavigation")]
        public virtual ICollection<AcessoPavimento> AcessoPavimento { get; set; }
    }
}
