using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.ViewModel
{
    public class Notificacao
    {
        public int IdNotificacao { get; set; }
        public int IdCandidato { get; set; }
        public string Titulo { get; set; }
        public string Corpo { get; set; }
        public string DataNotificacao { get; set; }
        public string Rota { get; set; }
        public bool Visualizado { get; set; }
        public string QueryParams { get; set; }


    }
}
