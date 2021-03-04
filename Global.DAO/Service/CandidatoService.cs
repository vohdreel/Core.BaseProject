using Global.DAO.Context;
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
    public class CandidatoService : IDisposable
    {
        private CandidatoRepository Repository { get; set; }

        public CandidatoService()
        {

            Repository = new CandidatoRepository();

        }

        public CandidatoService(GlobalContext context)
        {
            Repository = new CandidatoRepository(context);
        }


        public Candidato BuscarCandidato(int IdCandidato) 
        {

            return Repository.Get(x => x.Id == IdCandidato).FirstOrDefault();


        }

        public bool CadastrarCandidato(Candidato candidato)
        {
            return Repository.Insert(candidato);
        
        
        }

        public bool AtualizarCandidato(Candidato candidato)
        {
            return Repository.Update(candidato);


        }

        public Candidato BuscarCandidato(string IdAspNetUsers)
        {

            return Repository.Get(x => x.IdAspNetUsers == IdAspNetUsers).FirstOrDefault();


        }

        public bool ExisteCpfUsuario(string cpf)
        {

            return Repository.Get(x => x.Cpf == cpf).FirstOrDefault() != null;


        }

        public void AlternarMaterConectado(string IdAspNetUsers, bool value)
        {
            Candidato candidato = Repository.Get(x => x.IdAspNetUsers == IdAspNetUsers).FirstOrDefault();
            candidato.MaterConectado = value;
            Repository.Update(candidato); 

        }

        public bool VerificarManterConectado(int IdCandidato)
        {
            Candidato candidato = Repository.Get(x => x.Id == IdCandidato).FirstOrDefault();
            return candidato.MaterConectado;

        }


        public Coordinates BuscarCoordenadasCandidato(int idCandidato) {
            Candidato candidato = BuscarCandidato(idCandidato);

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

            bool resultado = Repository.Update(candidato);

            return candidato;

        }

        public Candidato[] VerificarCandidatoSemUsuario()
        {
            return Repository.Get(x => string.IsNullOrEmpty(x.IdAspNetUsers)).ToArray();

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
