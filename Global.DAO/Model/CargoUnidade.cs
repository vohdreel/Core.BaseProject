using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class CargoUnidade
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        public int Quantidade { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime HorarioInicio { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime HorarioFim { get; set; }
        [Column("IDUnidade")]
        public Guid Idunidade { get; set; }
        [Column("IDCargo")]
        public Guid? Idcargo { get; set; }
        [Column("StatusID")]
        public Guid StatusId { get; set; }

        [ForeignKey(nameof(Idunidade))]
        [InverseProperty(nameof(Unidade.CargoUnidade))]
        public virtual Unidade IdunidadeNavigation { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty("CargoUnidade")]
        public virtual Status Status { get; set; }
    }
}
