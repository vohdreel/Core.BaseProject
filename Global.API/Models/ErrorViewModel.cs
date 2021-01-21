using System;

namespace Global.API.Models
{
    public class ErrorViewModel
    {
                
        public int ErroCode { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }

        /*
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        */
    }
}
