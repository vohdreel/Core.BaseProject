using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class OpcaoAcesso
    {
        public OpcaoAcesso()
        {
            PavimentoOpcaoAcesso = new HashSet<PavimentoOpcaoAcesso>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        [InverseProperty("OpcaoAcesso")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdopcaoAcessoNavigation")]
        public virtual ICollection<PavimentoOpcaoAcesso> PavimentoOpcaoAcesso { get; set; }
    }
}
