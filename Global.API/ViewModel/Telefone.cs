using Global.Util;
using Global.Util.SystemEnumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.ViewModel
{
    public class Telefone
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int TipoTelefone { get; set; }

        public string StringTipoTelefone { get; set; }
        public Telefone(Global.DAO.Model.Telefone telefone)
        {
            this.Id = telefone.Id;
            this.Numero = telefone.Numero;
            this.StringTipoTelefone = EnumExtensions.GetEnumDisplayName((EnumTipoTelefone)telefone.TipoTelefone);
            this.TipoTelefone = telefone.TipoTelefone.Value;




    }
    }

    

}


