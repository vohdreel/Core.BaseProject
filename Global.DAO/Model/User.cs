using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class User
    {
        public User()
        {
            Machine = new HashSet<Machine>();
        }

        [Key]
        public int Id { get; set; }
        public int Name { get; set; }

        [InverseProperty("IdUserNavigation")]
        public virtual ICollection<Machine> Machine { get; set; }
    }
}
