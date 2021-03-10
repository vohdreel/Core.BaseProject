using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Global.DAO.Model
{
    public partial class Banner
    {
        public int Id { get; set; }
        public string Base64Code { get; set; }
        public string UrlRedirect { get; set; }
    }
}
