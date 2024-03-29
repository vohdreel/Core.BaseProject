﻿using Global.DAO.Context;
using Global.DAO.Model;
using Global.DAO.Procedure.Models;
using Global.DAO.Repository;
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

        public Candidato BuscarCandidato(string IdAspNetUsers)
        {

            return Repository.Get(x => x.IdAspNetUsers == IdAspNetUsers).FirstOrDefault();


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
