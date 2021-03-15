
using Global.DAO.Context;
using Global.DAO.Model;
using Global.DAO.Procedure.Models;
using Global.DAO.Repository;
using Global.Util;
using Global.Util.SystemEnumerations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Service
{
    public class VagaService : IDisposable
    {
        private VagaRepository Repository { get; set; }

        public VagaService()
        {

            Repository = new VagaRepository();

        }

        public VagaService(GlobalContext context)
        {
            Repository = new VagaRepository(context);
        }


        public Vaga Buscar(int Id)
        {

            return Repository
                .Get(x => x.Id == Id, includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation")
                .FirstOrDefault();

        }


        public Vaga[] BuscarVagasGerais()
        {

            return Repository
                .Get(includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation,VagaFavorita")
                .OrderByDescending(x => x.Id)
                .Take(5)
                .ToArray();

        }

        public Vaga[] BuscarVagasCampoDeBusca(string stringBusca)
        {
            List<Vaga> result = new List<Vaga>();
            List<Vaga> vagas = Repository.Get(includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation.IdEnumAgrupamentoNavigation").ToList();
            foreach (var vaga in vagas)
            {
                string vagaNome = vaga.IdCargoNavigation.NomeCargo.RemoveDiacritics().ToLower();
                string vagaEmpresa = vaga.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia.RemoveDiacritics().ToLower();
                string vagaAreas = vaga.IdCargoNavigation.IdEnumAgrupamentoNavigation.NomeAgrupamento.RemoveDiacritics().ToLower();
                if (vagaNome.Contains(stringBusca.RemoveDiacritics().ToLower()) ||
                    vagaEmpresa.Contains(stringBusca.RemoveDiacritics().ToLower()) ||
                    vagaAreas.Contains(stringBusca.RemoveDiacritics().ToLower()))
                    result.Add(vaga);
            }

            return result.ToArray();

        }

        public Vaga[] BuscarVagasCampoDeBuscaAntigas(string stringBusca, int idUltimaVaga)
        {
            List<Vaga> result = new List<Vaga>();
            List<Vaga> vagas = Repository.Get(x => x.Id < idUltimaVaga, includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation.IdEnumAgrupamentoNavigation").ToList();
            foreach (var vaga in vagas)
            {
                string vagaNome = vaga.IdCargoNavigation.NomeCargo.RemoveDiacritics().ToLower();
                string vagaEmpresa = vaga.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia.RemoveDiacritics().ToLower();
                string vagaAreas = vaga.IdCargoNavigation.IdEnumAgrupamentoNavigation.NomeAgrupamento.RemoveDiacritics().ToLower();
                if (vagaNome.Contains(stringBusca.RemoveDiacritics().ToLower()) ||
                    vagaEmpresa.Contains(stringBusca.RemoveDiacritics().ToLower()) ||
                    vagaAreas.Contains(stringBusca.RemoveDiacritics().ToLower()))
                    result.Add(vaga);
            }

            return result.ToArray();

        }

        public Vaga[] BuscarVagasGeraisAntigas(int idUltimaVaga)
        {
            return Repository
                    .Get(x => x.Id < idUltimaVaga, includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation")
                    .OrderByDescending(x => x.Id)
                    .Take(5)
                    .ToArray();


        }

        public Vaga[] BuscarVagasGeraisRecentes(int idPrimeiraVaga)
        {
            return Repository
                    .Get(x => x.Id > idPrimeiraVaga, includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation")
                    .OrderByDescending(x => x.Id)
                    .Take(5)
                    .ToArray();


        }

        public Vaga[] BuscarVagasPorDistancia(Coordinates coordenadasCandidato, int distanciaMinima, int distanciaMaxima, int salarioMinimo, int salarioMaximo)
        {
            List<Vaga> vagasCompativies = new List<Vaga>();
            List<Vaga> vagas = Repository
                .Get(includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation,VagaFavorita").ToList();

            foreach (var vaga in vagas)
            {
                Vaga _vaga = vaga;

                //Se a vaga ainda não possui latitude ou longitude, preencher usando o helper de coordenadas
                if (string.IsNullOrEmpty(vaga.Latitude) || string.IsNullOrEmpty(vaga.Longitude))
                {
                    _vaga = PreencherCoordenadas(_vaga);
                    //se ainda estiver vazia, pular essa comparação
                    if (string.IsNullOrEmpty(vaga.Latitude) || string.IsNullOrEmpty(vaga.Longitude))
                        continue;
                }

                Coordinates coordenadaVaga = new Coordinates(_vaga.Latitude, _vaga.Longitude);
                double distancia = GeoCoordinationExtension
                    .GetDistanceBetweenLocations(coordenadasCandidato, coordenadaVaga);

                if (distancia >= distanciaMinima && distancia <= distanciaMaxima && _vaga.Salario >= salarioMinimo && _vaga.Salario <= salarioMaximo)
                {
                    vagasCompativies.Add(_vaga);
                }

            }

            return vagasCompativies.ToArray();
        }

        public Vaga[] BuscarVagasDirecionadas(int idCandidato, int nivelMinimoCompatibilidade)
        {
            List<Vaga> vagasCompativies = new List<Vaga>();
            VagaCompatibilidade[] vagasCompatibilidade = Repository
                .GetContext()
                .Set<VagaCompatibilidade>()
                .FromSqlInterpolated($"EXEC [ObterVagasDirecionadas] @IdCandidato={idCandidato}")
                .AsEnumerable()
                .Where(x => x.Compatibilidade >= nivelMinimoCompatibilidade)
                .OrderByDescending(x => x.IdVaga)
                .Take(5)
                .ToArray();

            foreach (var resultado in vagasCompatibilidade)
                vagasCompativies.Add(Buscar(resultado.IdVaga));

            return vagasCompativies.ToArray();

        }

        public Vaga[] BuscarVagasDirecionadasAntigas(int idCandidato, int idUltimaVaga, int nivelMinimoCompatibilidade)
        {
            List<Vaga> vagasCompativies = new List<Vaga>();
            VagaCompatibilidade[] vagasCompatibilidade = Repository
                .GetContext()
                .Set<VagaCompatibilidade>()
                .FromSqlInterpolated($"EXEC [ObterVagasDirecionadas] @IdCandidato={idCandidato}")
                .AsEnumerable()
                .Where(x => x.Compatibilidade >= nivelMinimoCompatibilidade && x.IdVaga < idUltimaVaga)
                .OrderByDescending(x => x.IdVaga)
                .Take(5)
                .ToArray();

            foreach (var resultado in vagasCompatibilidade)
                vagasCompativies.Add(Buscar(resultado.IdVaga));

            return vagasCompativies.ToArray();

        }

        public Vaga[] BuscarVagasDirecionadasRecentes(int idCandidato, int idPrimeiraVaga, int nivelMinimoCompatibilidade)
        {
            List<Vaga> vagasCompativies = new List<Vaga>();
            VagaCompatibilidade[] vagasCompatibilidade = Repository
                .GetContext()
                .Set<VagaCompatibilidade>()
                .FromSqlInterpolated($"EXEC [ObterVagasDirecionadas] @IdCandidato={idCandidato}")
                .AsEnumerable()
                .Where(x => x.Compatibilidade >= nivelMinimoCompatibilidade && x.IdVaga > idPrimeiraVaga)
                .OrderByDescending(x => x.IdVaga)
                .Take(5)
                .ToArray();

            foreach (var resultado in vagasCompatibilidade)
                vagasCompativies.Add(Buscar(resultado.IdVaga));

            return vagasCompativies.ToArray();

        }




        public string MontarVagaEndereco(Vaga vaga)
        {
            string fullAddress = "";
            if (string.IsNullOrEmpty(vaga.Endereco)) return "";
            else
            {
                fullAddress += vaga.Endereco.Trim();
                if (!string.IsNullOrEmpty(vaga.Numero))
                    fullAddress += (", " + vaga.Numero.Trim());
                if (!string.IsNullOrEmpty(vaga.Cep))
                    fullAddress += (" - " + vaga.Cep.Trim());

            }
            fullAddress += vaga.Cidade + ", " + vaga.Estado;

            return fullAddress;


        }

        public string MontarEmpresLogo(Vaga vaga)
        {
            string extensao = vaga.IdProcessoSeletivoNavigation.IdEmpresaNavigation.ImageLogoExtension;
            string base64 = vaga.IdProcessoSeletivoNavigation.IdEmpresaNavigation.Base64ImageLogo;

            if (!string.IsNullOrEmpty(extensao) && !string.IsNullOrEmpty(base64))
                return extensao + ',' + base64;
            else
                return "https://via.placeholder.com/50x50";

        }


        public bool Salvar(Vaga Dados)
        {

            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(Vaga Dados)
        {

            bool resultado = Repository.Update(Dados);

            return resultado;

        }

        public Vaga PreencherCoordenadas(Vaga dados)
        {
            string fullAddress = "";
            //se nao houver endereço, impossivel
            if (string.IsNullOrEmpty(dados.Endereco)) return dados;
            else
            {
                fullAddress += dados.Endereco.Trim().Replace(" ", "+");
                if (!string.IsNullOrEmpty(dados.Numero))
                {
                    fullAddress += (",+" + dados.Numero.Trim());
                    if (!string.IsNullOrEmpty(dados.Cep))
                    {
                        fullAddress += ("+-+" + dados.Cep.Trim());
                    }
                }
            }

            string key = "AIzaSyBDZN9proIwpDe18stl_EzVQjnxYTbdQLY";

            Coordinates coordinates = GeoCoordinationExtension
                .GetCoordinatesFromApi(fullAddress, key);

            dados.Latitude = coordinates?.Latitude.ToString() ?? null;
            dados.Longitude = coordinates?.Longitude.ToString() ?? null;

            bool resultado = Repository.Update(dados);

            return dados;

        }

        public bool Excluir(int Id)
        {
            var dados = Repository.GetByID(Id);
            bool resultado = Repository.Delete(dados);

            return resultado;
        }


        public bool VerificarVagaPorReferenceNumber(int referenceNumber)
        {
            return Repository.Get(x => x.ReferenceNumber == referenceNumber).FirstOrDefault() != null;


        }

        public GlobalContext GetContext()
        {
            return Repository.GetContext();
        }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
}
