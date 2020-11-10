using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Trace
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [StringLength(300)]
        public string UserName { get; set; }
        [Required]
        public string Command { get; set; }
        [Required]
        [StringLength(200)]
        public string Table { get; set; }
        [Required]
        [Column("Command_Type")]
        [StringLength(10)]
        public string CommandType { get; set; }
        [Required]
        [Column("Command_Parameter")]
        public string CommandParameter { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        public Guid? UserId { get; set; }
    }
}
