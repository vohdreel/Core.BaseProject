﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Log
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        public string CallSite { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }
        public string Exception { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string MachineName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Thread { get; set; }
        public string Username { get; set; }
    }
}
