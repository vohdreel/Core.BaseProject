using Global.Util;
using Global.Util.SystemEnumerations;
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
        public byte[] ByteArray { get; set; }
        public string Extensao { get; set; }
        public string NomeArquivo { get; set; }
        public string Base64String { get; set; }

        public Documento(Global.DAO.Model.Documento documento)
        {
            IdArquivo = documento.Id;
            IdEnumTipoDocumento = documento.IdEnumTipoDocumento;
            TipoDocumento = EnumExtensions.GetEnumDisplayName((NomeTipoDocumento)documento.IdEnumTipoDocumento);
            IdCandidato = documento.IdCandidato;
            ByteArray = documento.ByteArray;
            Extensao = documento.Extensao;
            NomeArquivo = documento.NomeArquivo;
            Base64String = documento.Base64Code;
        }

    }
}
