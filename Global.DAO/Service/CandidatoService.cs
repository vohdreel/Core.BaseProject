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
            return new Coordinates(candidato.Latitude, candidato.Longitude);

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
