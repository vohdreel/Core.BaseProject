using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class AcessoPavimento
    {
        public AcessoPavimento()
        {
            PavimentoOpcaoAcesso = new HashSet<PavimentoOpcaoAcesso>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        public int QuantidadeEntrada { get; set; }
        public int QuantidadeSaida { get; set; }
        [Column("QuantidadeEntrada_Saida")]
        public int QuantidadeEntradaSaida { get; set; }
        public int QuantidadeVagas { get; set; }
        [Column("IDPavimentoUnidade")]
        public Guid IdpavimentoUnidade { get; set; }
        [Column("StatusID")]
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(IdpavimentoUnidade))]
        [InverseProperty(nameof(PavimentoUnidade.AcessoPavimento))]
        public virtual PavimentoUnidade IdpavimentoUnidadeNavigation { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("AcessoPavimento")]
        public virtual Status Status { get; set; }
        [InverseProperty("IdpavimentoAcessoNavigation")]
        public virtual ICollection<PavimentoOpcaoAcesso> PavimentoOpcaoAcesso { get; set; }
    }
}
