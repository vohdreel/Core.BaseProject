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
    public class VagaController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("GetVagasGeraisPaginaInicial")]
        public ViewModel.Vaga[] GetVagasGeraisPaginaInicial()
        {
            using (var service = new VagaService())
            {

                var teste = service.BuscarVagasGerais();


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
                    Beneficios = x.Beneficios,
                    Favoritado = x.VagaFavorita.Any() && x.VagaFavorita.Where(y => y.IdCandidato == 2).Any(),
                    Endereco = service.MontarVagaEndereco(x)

                }).ToArray();

                return vagas;

            }
        }

        [AllowAnonymous]
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
                    Beneficios = x.Beneficios,
                    Favoritado = x.VagaFavorita.Any() && x.VagaFavorita.Where(y => y.IdCandidato == 2).Any(),
                    Endereco = service.MontarVagaEndereco(x)

                }).ToArray();
                return vagas;

            }
        }

        [AllowAnonymous]
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
                    Beneficios = x.Beneficios,
                    Favoritado = x.VagaFavorita.Any() && x.VagaFavorita.Where(y => y.IdCandidato == 2).Any(),
                    Endereco = service.MontarVagaEndereco(x)

                }).ToArray();
                return vagas;

            }
        }


        [HttpGet("SearchVagas")]
        public ViewModel.Vaga[] GetVagasCampoDeBusca(string stringBusca)
        {
            using (var service = new VagaService())
            {
                if (string.IsNullOrEmpty(stringBusca))
                    return null;

                var vagas = service.BuscarVagasCampoDeBusca(stringBusca).Select(x => new ViewModel.Vaga
                {
                    IdVaga = x.Id,
                    NomeCargo = x.IdCargoNavigation.NomeCargo,
                    NomeEmpresa = x.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                    Salario = x.Salario?.ToString("c"),
                    Modalidade = ((VagaModalidade)x.Modalidade).ToString(),
                    Cidade = x.Cidade,
                    Estado = x.Estado,
                    Requisitos = x.Requisitos,
                    Beneficios = x.Beneficios,
                    Favoritado = x.VagaFavorita.Any() && x.VagaFavorita.Where(y => y.IdCandidato == 2).Any(),
                    Endereco = service.MontarVagaEndereco(x)

                }).ToArray();
                return vagas;

            }
        }

        [HttpGet("SearchVagasAntigas")]
        public ViewModel.Vaga[] GetVagasCampoDeBuscaAntigas(string stringBusca, int idUltimaVaga)
        {
            using (var service = new VagaService())
            {
                var vagas = service.BuscarVagasCampoDeBuscaAntigas(stringBusca, idUltimaVaga).Select(x => new ViewModel.Vaga
                {
                    IdVaga = x.Id,
                    NomeCargo = x.IdCargoNavigation.NomeCargo,
                    NomeEmpresa = x.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                    Salario = x.Salario?.ToString("c"),
                    Modalidade = ((VagaModalidade)x.Modalidade).ToString(),
                    Cidade = x.Cidade,
                    Estado = x.Estado,
                    Requisitos = x.Requisitos,
                    Beneficios = x.Beneficios,
                    Favoritado = x.VagaFavorita.Any() && x.VagaFavorita.Where(y => y.IdCandidato == 2).Any(),
                    Endereco = service.MontarVagaEndereco(x)

                }).ToArray();
                return vagas;

            }
        }


        [HttpGet("GetVagasPorDistancia")]
        public ViewModel.Vaga[] GetVagasPorDistancia(int idCandidato, int distanciaMinima, int distanciaMaxima)
        {

            using (var candidatoService = new CandidatoService())
            using (var service = new VagaService())
            {
                Coordinates coordenadasCandidato = candidatoService.BuscarCoordenadasCandidato(idCandidato);

                var vagas = service
                    .BuscarVagasPorDistancia(coordenadasCandidato, distanciaMinima, distanciaMaxima)
                    .Select(x => new ViewModel.Vaga
                    {
                        IdVaga = x.Id,
                        NomeCargo = x.IdCargoNavigation.NomeCargo,
                        NomeEmpresa = x.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                        Salario = x.Salario?.ToString("c"),
                        Modalidade = ((VagaModalidade)x.Modalidade).ToString(),
                        Cidade = x.Cidade,
                        Estado = x.Estado,
                        Requisitos = x.Requisitos,
                        Beneficios = x.Beneficios,
                        Favoritado = x.VagaFavorita.Any() && x.VagaFavorita.Where(y => y.IdCandidato == 2).Any(),
                        Endereco = service.MontarVagaEndereco(x)

                    }).ToArray();
                return vagas;

            }

        }




        [HttpGet("GetVagasFavoritasCandidato")]
        public ViewModel.Vaga[] GetVagasFavoritasCandidato(int idCandidato)
        {
            using (var service = new VagaFavoritaService())
            {
                var vagas = service.BuscarVagasFavoritasCandidato(idCandidato)
                    .Select(x => new ViewModel.Vaga
                    {
                        IdVaga = x.Id,
                        NomeCargo = x.IdCargoNavigation.NomeCargo,
                        NomeEmpresa = x.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                        Salario = x.Salario?.ToString("c"),
                        Modalidade = ((VagaModalidade)x.Modalidade).ToString(),
                        Cidade = x.Cidade,
                        Estado = x.Estado,
                        Requisitos = x.Requisitos,
                        Beneficios = x.Beneficios,
                        Favoritado = true

                    }).ToArray();
                return vagas;

            }
        }
        [HttpGet("GetVagaPorId")]
        public ViewModel.Vaga GetVagaPorId(int idVaga, int idCandidato)
        {
            using (var vagaFavoritaService = new VagaFavoritaService())
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
                    Beneficios = vaga.Beneficios,
                    Favoritado = vagaFavoritaService.IsVagaFavoritadaPorCandidato(idCandidato, vaga.Id),
                    Endereco = service.MontarVagaEndereco(vaga)
                };


            }


        }

        [HttpPost("FavoritarVaga")]
        public bool FavoritarVaga([FromBody] VagaFavorita vagaFavorita)
        {
            var service = new VagaFavoritaService();
            bool sucesso = service.Salvar(vagaFavorita);

            return sucesso;
        }

        [HttpPut("DesfavoritarVaga")]
        public bool DesfavoritarVaga([FromBody] VagaFavorita vagaFavorita)
        {
            var service = new VagaFavoritaService();
            bool sucesso = service.ExcluirPorIdCandidatoIdVaga(vagaFavorita.IdCandidato, vagaFavorita.IdVaga);

            return sucesso;
        }
    }
}
