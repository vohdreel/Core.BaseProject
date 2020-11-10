﻿using System;
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
        public int IdUser { get; set; }

        [ForeignKey(nameof(IdUser))]
        [InverseProperty(nameof(User.Machine))]
        public virtual User IdUserNavigation { get; set; }
    }
}
