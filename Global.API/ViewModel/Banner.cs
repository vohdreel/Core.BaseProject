using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.ViewModel
{
    public class Banner
    {
        public int Id { get; set; }
        public string Base64Code { get; set; }
        public string UrlRedirect { get; set; }

        public Banner(Global.DAO.Model.Banner banner)
        {
            Id = banner.Id;
            Base64Code = "data:image/png;base64," + banner.Base64Code;
            UrlRedirect = banner.UrlRedirect;
        
        
        
        
        }
    
    }
}
