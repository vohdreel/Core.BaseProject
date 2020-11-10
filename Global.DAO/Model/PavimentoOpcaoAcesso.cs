using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class PavimentoOpcaoAcesso
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Column("IDPavimentoAcesso")]
        public Guid IdpavimentoAcesso { get; set; }
        [Column("IDOpcaoAcesso")]
        public Guid IdopcaoAcesso { get; set; }

        [ForeignKey(nameof(IdopcaoAcesso))]
        [InverseProperty(nameof(OpcaoAcesso.PavimentoOpcaoAcesso))]
        public virtual OpcaoAcesso IdopcaoAcessoNavigation { get; set; }
        [ForeignKey(nameof(IdpavimentoAcesso))]
        [InverseProperty(nameof(AcessoPavimento.PavimentoOpcaoAcesso))]
        public virtual AcessoPavimento IdpavimentoAcessoNavigation { get; set; }
    }
}
