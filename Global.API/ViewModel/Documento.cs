using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.ViewModel
{
    public class Documento
    {

        public int IdArquivo { get; set; }
        public int IdEnumTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public int IdCandidato { get; set; }
        public string ByteArray { get; set; }
        public string Extensao { get; set; }
        public string NomeArquivo { get; set; }



    }
}
