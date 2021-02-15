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
            List<Vaga> vagas = Repository.Get(includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation").ToList();
            foreach (var vaga in vagas)
            {
                string vagaNome = vaga.IdCargoNavigation.NomeCargo.RemoveDiacritics().ToLower();
                if (vagaNome.Contains(stringBusca.RemoveDiacritics().ToLower()))
                    result.Add(vaga);
            }
            
            return result.ToArray();

        }

        public Vaga[] BuscarVagasCampoDeBuscaAntigas(string stringBusca, int idUltimaVaga)
        {
            return Repository
                       .Get(x =>
                        x.Id < idUltimaVaga &&
                       x.IdCargoNavigation
                       .NomeCargo
                       .RemoveDiacritics()
                       .ToLower()
                       .Contains(stringBusca.RemoveDiacritics().ToLower())
                       , includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation")
                       .OrderByDescending(x => x.Id)
                       .Take(10)
                       .ToArray();

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

        public Vaga[] BuscarVagasPorDistancia(Coordinates coordenadasCandidato, int distanciaMinima, int distanciaMaxima)
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

                if (distancia >= distanciaMinima && distancia <= distanciaMaxima)
                {
                    vagasCompativies.Add(_vaga);
                }

            }

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
                {
                    fullAddress += (", " + vaga.Numero.Trim());
                    if (!string.IsNullOrEmpty(vaga.Cep))
                    {
                        fullAddress += (" - " + vaga.Cep.Trim());
                    }
                }
            }

            return fullAddress;


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
