using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Machine
    {
        [Key]
        public int Id { get; set; }
        public int Name { get; set; }
        public bool IsActive { get; set; }
    }
}
