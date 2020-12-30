using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.DAO.Model;
using Global.DAO.Service;
using Global.Util.SystemEnumerations;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("[area]/[controller]")]
    public class VagaController : ControllerBase
    {
     
        [HttpGet("GetVagasGeraisPaginaInicial")]
        public ViewModel.Vaga[] GetVagasGeraisPaginaInicial()
        {
            using (var service = new VagaService())
            {
                var vagas = service.BuscarVagasGerais().Select(x => new ViewModel.Vaga
                {
                    IdVaga = x.Id,
                    NomeCargo = x.IdCargoNavigation.NomeCargo,
                    NomeEmpresa = x.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                    Salario = x.Salario?.ToString("c"),
                    Modalidade = ((VagaModalidade)x.Modalidade).ToString(),
                    Cidade = x.Cidade,
                    Estado = x.Estado,
                    Requisitos = x.Requisitos,
                    Beneficios = x.Beneficios

                }).ToArray();
                return vagas;

            }
        }
        [HttpGet("GetVagasGeraisRecentes")]
        public ViewModel.Vaga[] GetVagasGeraisRecentes(int idPrimeiraVaga)
        {
            using (var service = new VagaService())
            {
                var vagas = service.BuscarVagasGeraisRecentes(idPrimeiraVaga).Select(x => new ViewModel.Vaga
                {
                    IdVaga = x.Id,
                    NomeCargo = x.IdCargoNavigation.NomeCargo,
                    NomeEmpresa = x.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                    Salario = x.Salario?.ToString("c"),
                    Modalidade = ((VagaModalidade)x.Modalidade).ToString(),
                    Cidade = x.Cidade,
                    Estado = x.Estado,
                    Requisitos = x.Requisitos,
                    Beneficios = x.Beneficios

                }).ToArray();
                return vagas;

            }
        }

        [HttpGet("GetVagasGeraisAntigas")]
        public ViewModel.Vaga[] GetVagasGeraisAntigas(int idUltimaVaga)
        {
            using (var service = new VagaService())
            {
                var vagas = service.BuscarVagasGeraisAntigas(idUltimaVaga).Select(x => new ViewModel.Vaga
                {
                    IdVaga = x.Id,
                    NomeCargo = x.IdCargoNavigation.NomeCargo,
                    NomeEmpresa = x.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                    Salario = x.Salario?.ToString("c"),
                    Modalidade = ((VagaModalidade)x.Modalidade).ToString(),
                    Cidade = x.Cidade,
                    Estado = x.Estado,
                    Requisitos = x.Requisitos,
                    Beneficios = x.Beneficios

                }).ToArray();
                return vagas;

            }
        }
        [HttpGet("GetVagaPorId")]

        public ViewModel.Vaga GetVagaPorId(int idVaga)
        {

            using (var service = new VagaService())
            {

                var vaga = service.Buscar(idVaga);

                return new ViewModel.Vaga()
                {

                    IdVaga = vaga.Id,
                    NomeCargo = vaga.IdCargoNavigation.NomeCargo,
                    NomeEmpresa = vaga.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                    Salario = vaga.Salario?.ToString("c"),
                    Modalidade = ((VagaModalidade)vaga.Modalidade).ToString(),
                    Cidade = vaga.Cidade,
                    Estado = vaga.Estado,
                    Requisitos = vaga.Requisitos,
                    Beneficios = vaga.Beneficios

                };
            
            
            }


        }
    }
}
