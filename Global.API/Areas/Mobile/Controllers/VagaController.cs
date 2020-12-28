using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.DAO.Model;
using Global.DAO.Service;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    public class VagaController : ControllerBase
    {
        public Vaga[] GetVagasGeraisPaginaInicial()
        {
            using (var service = new VagaService())
            {
                var vagas = service.BuscarVagasGerais();
                return vagas;                                
            
            }
        }
    }
}
