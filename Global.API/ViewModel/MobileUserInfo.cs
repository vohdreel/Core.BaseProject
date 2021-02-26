using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.ViewModel
{
    public class MobileUserInfo
    {
    }

    public class Preferencias
    {
        public CargoInteresse[] cargosInteresse { get; set; }
        public AreaInteresse[] areasInteresse { get; set; }

    }
}
