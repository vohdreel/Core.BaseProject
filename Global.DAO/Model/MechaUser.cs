using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class MechaUser
    {
        public MechaUser()
        {
            Machine = new HashSet<Machine>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<Machine> Machine { get; set; }
    }
}
