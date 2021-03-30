using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Logger
    {
        [Key]
        public int Id { get; set; }
        public string TextoLog { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DataLog { get; set; }
    }
}
