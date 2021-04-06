using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.DAO.Model;
using Global.DAO.Service;
using Global.Util;
using Global.Util.SystemEnumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class CandidaturaController : ControllerBase
    {

        [HttpGet("ListarCandidaturas")]
        public ViewModel.Candidatura[] ListarCandidaturas(int idCandidato)
        {
            using (var vagaService = new VagaService())
            using (var service = new CandidaturaService())
            {
                return service.ListarPorCandidato(idCandidato)
                    .Select(x => new ViewModel.Candidatura
                    {
                        IdCandidatura = x.Id,
                        NomeCargo = x.IdVagaNavigation.IdCargoNavigation.NomeCargo,
                        NomeEmpresa = x.IdVagaNavigation.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                        AngularRoute = "tab3/modal-vaga",
                        QueryParams = "{\"idVaga\":" + x.IdVaga + "}",
                        Situacao = EnumExtensions.GetEnumDisplayName((StatusCandidatura)x.StatusCandidatura),
                        EmpresaLogo = vagaService.MontarEmpresLogo(x.IdVagaNavigation)

                    }).ToArray();

            }
        }
        [HttpGet("ListarCandidaturasAntigas")]
        public object[] ListarCandidaturasAntigas(int idCandidato, int idUltimaCandidatura)
        {
            using (var vagaService = new VagaService())
            using (var service = new CandidaturaService())
            {
                return service.ListarPorCandidatoAntigas(idCandidato, idUltimaCandidatura)
                    .Select(x => new ViewModel.Candidatura
                    {
                        IdCandidatura = x.Id,
                        NomeCargo = x.IdVagaNavigation.IdCargoNavigation.NomeCargo,
                        NomeEmpresa = x.IdVagaNavigation.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                        AngularRoute = "tab3/modal-vaga",
                        QueryParams = "{\"idVaga\":" + x.IdVaga + "}",
                        Situacao = EnumExtensions.GetEnumDisplayName((StatusCandidatura)x.StatusCandidatura),
                        EmpresaLogo = vagaService.MontarEmpresLogo(x.IdVagaNavigation)

                    }).ToArray();

            }
        }

        [HttpGet("ListarCandidaturasRecentes")]
        public object[] ListarCandidaturasRecentes(int idCandidato, int idPrimeiraCandidatura)
        {
            using (var vagaService = new VagaService())
            using (var service = new CandidaturaService())
            {
                return service.ListarPorCandidatoRecentes(idCandidato, idPrimeiraCandidatura)
                    .Select(x => new ViewModel.Candidatura
                    {
                        IdCandidatura = x.Id,
                        NomeCargo = x.IdVagaNavigation.IdCargoNavigation.NomeCargo,
                        NomeEmpresa = x.IdVagaNavigation.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                        AngularRoute = "tab3/modal-vaga",
                        QueryParams = "{\"idVaga\":" + x.IdVaga + "}",
                        Situacao = EnumExtensions.GetEnumDisplayName((StatusCandidatura)x.StatusCandidatura),
                        EmpresaLogo = vagaService.MontarEmpresLogo(x.IdVagaNavigation)

                    }).ToArray();

            }
        }




        [HttpPost("GerarCandidatura")]
        public object GerarCandidatura([FromBody] Candidatura candidatura)
        {
            using (var candidadoService = new CandidatoService())
            using (var service = new CandidaturaService())
            {

                Candidato candidato = candidadoService.BuscarCandidato(candidatura.IdCandidato);

                if (!(candidato.InformacoesPessoaisConcluido.Value && candidato.ObjetivosConcluido.Value && !string.IsNullOrEmpty(candidato.Idlegado)))
                {
                    return new
                    {
                        ok = false,
                        message = "Termine de cadastrar suas informações para candidatar-se à esta vaga!"
                    };

                }

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
                        message = "Voce já se candidatou para essa vaga! Escolha uma vaga diferente"
                    };




            }

        }

    }
}
