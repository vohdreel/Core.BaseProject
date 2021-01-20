using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class EnumPais
    {
        [Key]
        [StringLength(5)]
        public string CodigoPais { get; set; }
        [Required]
        [StringLength(60)]
        public string Pais { get; set; }
        [StringLength(5)]
        public string SiglaPais { get; set; }
    }
}
