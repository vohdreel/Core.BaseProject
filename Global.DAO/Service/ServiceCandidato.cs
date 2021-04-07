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


        public ServiceCandidato(IRepositoryCandidato repositoryCandidato)
        {

            RepositoryCandidato = repositoryCandidato;

        }

        public Candidato BuscarPorId(int IdCandidato) 
        {

            return RepositoryCandidato.Get(x => x.Id == IdCandidato).FirstOrDefault();


        }

        public IEnumerable<Candidato> Listar() {
            return RepositoryCandidato.Get();
        
        }
        public Candidato[] BuscarMassaDeCandidatos()
        {

            return Repository.Get(x => !string.IsNullOrEmpty(x.IdAspNetUsers) && !string.IsNullOrEmpty(x.SenhaCriptografada)).OrderByDescending(x=> x.Id).Take(100).ToArray();


        }

        public Candidato BuscarCandidato(int IdCandidato)
        public bool Salvar(Candidato candidato)
        {

            return Repository.Get(x => x.Id == IdCandidato, includeProperties: "TelefoneCandidato").FirstOrDefault();


            return RepositoryCandidato.Insert(candidato);
        
        
        }

        public Candidato BuscarCandidatoPorIdLegado(string IdLegado)
        {

            return Repository.Get(x => x.Idlegado == IdLegado).FirstOrDefault();


        }
        public bool CadastrarCandidato(Candidato candidato)
        {
            return Repository.Insert(candidato);


        }

        public bool CadastrarCandidato(Candidato candidato, out string exception)
        {
            return Repository.InsertWithException(candidato, out exception);


        }

        public bool AtualizarCandidato(Candidato candidato)
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

            return Repository.Get(x => x.Cpf == cpf.Replace(".", "").Replace("-", "")).FirstOrDefault() != null;


        }

        public bool ExisteIdlegadoUsuario(string idlegado)
        {

            return Repository.Get(x => x.Idlegado == idlegado).FirstOrDefault() != null;


        }

        public void DeleteCandidatosDuplicados()
        {

            Candidato[] candidatosDuplicados = Repository.Get(x => !string.IsNullOrEmpty(x.Idlegado) && string.IsNullOrEmpty(x.IdAspNetUsers));

            Repository.DeleteAll(candidatosDuplicados);
        
        
        }
        public void AlternarMaterConectado(string IdAspNetUsers, bool value)
        {
            Candidato candidato = RepositoryCandidato.Get(x => x.IdAspNetUsers == IdAspNetUsers).FirstOrDefault();
            candidato.MaterConectado = value;
            RepositoryCandidato.Update(candidato); 

        }

        public void AlternarFcmTokenConectado(string IdAspNetUsers, string fcmToken)
        {
            Candidato candidato = Repository.Get(x => x.IdAspNetUsers == IdAspNetUsers).FirstOrDefault();
            candidato.Fcmtoken = fcmToken;
            Repository.Update(candidato);

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
                    fullAddress += (", " + candidato.Numero.Trim());
                if (!string.IsNullOrEmpty(candidato.Cep))
                    fullAddress += (" - " + candidato.Cep.Trim());


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

        public Candidato[] VerificarCandidatoSemUsuario()
        {
            return Repository.Get(x => string.IsNullOrEmpty(x.IdAspNetUsers)).ToArray();

        }
        public GlobalContext GetContext()
        {
            return Repository.GetContext();
        }

    }
}
