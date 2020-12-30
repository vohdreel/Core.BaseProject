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
    [Route("[area]/[controller]")]
    public class CandidaturaController : ControllerBase
    {
        [HttpPost("GerarCandidatura")]
        public bool GerarCandidatura([FromBody] Candidatura candidatura)
        {
            using (var service = new CandidaturaService())
            {
                candidatura.DataInscricao = DateTime.UtcNow;
                bool result = service.Salvar(candidatura);
                return result;


            }

        }

    }
}
