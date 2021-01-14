using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.DAO.Model;
using Global.DAO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class CandidaturaController : ControllerBase
    {
        [HttpPost("GerarCandidatura")]
        public object GerarCandidatura([FromBody] Candidatura candidatura)
        {
            using (var service = new CandidaturaService())
            {

                if (!service.ExisteCandidatura(candidatura.IdVaga, candidatura.IdCandidato))
                {
                    candidatura.DataInscricao = DateTime.UtcNow;
                    bool result = service.Salvar(candidatura);
                    return new
                    {
                        ok = result,
                        message = !result ? "Ocorreu algum erro ao tentar se candidatar, tente novamente mais tarde!" : ""
                    };
                }

                else
                    return new
                    {
                        ok = false,
                        message =  "Voce já se candidatou para essa vaga! Escolha uma vaga diferente" 
                    };




            }

        }

    }
}
