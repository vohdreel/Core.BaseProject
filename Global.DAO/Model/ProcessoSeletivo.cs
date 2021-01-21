using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class ProcessoSeletivo
    {
        public ProcessoSeletivo()
        {
            Vaga = new HashSet<Vaga>();
        }

        [Key]
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        [Required]
        [StringLength(100)]
        public string NomeProcesso { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DataInicioProcesso { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DataTerminoProcesso { get; set; }
        public int StatusProcesso { get; set; }

        [ForeignKey(nameof(IdEmpresa))]
        [InverseProperty(nameof(Empresa.ProcessoSeletivo))]
        public virtual Empresa IdEmpresaNavigation { get; set; }
        [InverseProperty("IdProcessoSeletivoNavigation")]
        public virtual ICollection<Vaga> Vaga { get; set; }
    }
}
