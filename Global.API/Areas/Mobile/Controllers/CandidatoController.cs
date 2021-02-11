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
    public class CandidatoController : ControllerBase
    {
        [HttpPost("AtualizarInformacoesPessoais")]
        public object AtualizarInformacoesPessoais([FromBody]Candidato informacoesPessoais, [FromBody]int idCandidato)
        {
            using (var service = new CandidatoService())
            {

                Candidato candidato = service.BuscarCandidato(idCandidato);

                informacoesPessoais.Id = candidato.Id;
                informacoesPessoais.Cpf = candidato.Cpf;
                informacoesPessoais.Email = candidato.Email;
                informacoesPessoais.Nome = candidato.Nome;



                bool successo = service.AtualizarCandidato(informacoesPessoais);
                return new { ok = successo};



            }
        }



        [HttpGet("ObterInformacoesPessoais")]
        public object AtualizarInformacoesPessoais(int idCandidato)
        {
            using (var service = new CandidatoService())
            {

                Candidato candidado = service.BuscarCandidato(idCandidato);
                return candidado;

            
            
            }
        }
    }
}
