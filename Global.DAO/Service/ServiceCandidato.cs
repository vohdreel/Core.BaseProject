using Global.DAO.Context;
using Global.DAO.Interface.Repository;
using Global.DAO.Interface.Service;
using Global.DAO.Model;
using Global.DAO.Procedure.Models;
using Global.DAO.Repository;
using Global.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Service
{
    public class ServiceCandidato : IServiceCandidato
    {
        private readonly IRepositoryCandidato RepositoryCandidato;


        public ServiceCandidato(IRepositoryCandidato RepositoryCandidatoCandidato)
        {

            RepositoryCandidato = RepositoryCandidatoCandidato;

        }

        public Candidato BuscarPorId(int IdCandidato) 
        {

            return RepositoryCandidato.Get(x => x.Id == IdCandidato).FirstOrDefault();


        }

        public IEnumerable<Candidato> Listar() {
            return RepositoryCandidato.Get();
        
        }

        public bool Salvar(Candidato candidato)
        {
            return RepositoryCandidato.Insert(candidato);
        
        
        }

        public bool Atualizar(Candidato candidato)
        {
            return RepositoryCandidato.Update(candidato);

        }

        public bool Excluir(int IdCandidato)
        {
            bool resultado = RepositoryCandidato.Delete(IdCandidato);

            return resultado;

        }

        public Candidato BuscarPorIdAspNetUser(string IdAspNetUsers)
        {

            return RepositoryCandidato.Get(x => x.IdAspNetUsers == IdAspNetUsers).FirstOrDefault();


        }

        public bool ExisteCpfUsuario(string cpf)
        {

            return RepositoryCandidato.Get(x => x.Cpf == cpf).FirstOrDefault() != null;


        }

        public void AlternarMaterConectado(string IdAspNetUsers, bool value)
        {
            Candidato candidato = RepositoryCandidato.Get(x => x.IdAspNetUsers == IdAspNetUsers).FirstOrDefault();
            candidato.MaterConectado = value;
            RepositoryCandidato.Update(candidato); 

        }

        public bool VerificarManterConectado(int IdCandidato)
        {
            Candidato candidato = RepositoryCandidato.Get(x => x.Id == IdCandidato).FirstOrDefault();
            return candidato.MaterConectado;

        }


        public Coordinates BuscarCoordenadasCandidato(int idCandidato) {
            Candidato candidato = BuscarPorId(idCandidato);

            if (candidato.Latitude == null || candidato.Longitude == null)
            {
                candidato = PreencherCoordenadas(candidato);
                if (candidato.Latitude == null || candidato.Longitude == null)
                    return null;
            }
            return new Coordinates(candidato.Latitude, candidato.Longitude);

        }

        public string MontarVagaEndereco(Candidato candidato)
        {
            string fullAddress = "";
            if (string.IsNullOrEmpty(candidato.Endereco)) return "";
            else
            {
                fullAddress += candidato.Endereco.Trim();
                if (!string.IsNullOrEmpty(candidato.Numero))
                {
                    fullAddress += (", " + candidato.Numero.Trim());
                    if (!string.IsNullOrEmpty(candidato.Cep))
                    {
                        fullAddress += (" - " + candidato.Cep.Trim());
                    }
                }
            }

            return fullAddress;


        }

        public Candidato PreencherCoordenadas(Candidato candidato)
        {
            string fullAddress = "";
            //se nao houver endereço, impossivel
            if (string.IsNullOrEmpty(candidato.Endereco)) return candidato;
            else
            {
                fullAddress = MontarVagaEndereco(candidato);
            }

            string key = "AIzaSyBDZN9proIwpDe18stl_EzVQjnxYTbdQLY";

            Coordinates coordinates = GeoCoordinationExtension
                .GetCoordinatesFromApi(fullAddress, key);

            candidato.Latitude = coordinates?.Latitude.ToString() ?? null;
            candidato.Longitude = coordinates?.Longitude.ToString() ?? null;

            bool resultado = RepositoryCandidato.Update(candidato);

            return candidato;

        }

    }
}
