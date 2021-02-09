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
    public class CandidaturaService : IDisposable
    {
        private CandidaturaRepository Repository { get; set; }

        public CandidaturaService()
        {

            Repository = new CandidaturaRepository();

        }

        public CandidaturaService(GlobalContext context)
        {
            Repository = new CandidaturaRepository(context);
        }


        public Candidatura Buscar(int Id)
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }


        public Candidatura[] ListarPorCandidato(int IdCandidato)
        {
            return Repository.Get(x => x.IdCandidato == IdCandidato,
                includeProperties: "IdVagaNavigation,IdVagaNavigation.IdCargoNavigation,IdVagaNavigation.IdCargoNavigation,IdVagaNavigation.IdProcessoSeletivoNavigation.IdEmpresaNavigation")
                                    .OrderByDescending(x => x.Id)

                .Take(5)

                .ToArray();


        }

        public Candidatura[] ListarPorCandidatoAntigas(int IdCandidato, int IdUltimaCandidatura)
        {

            return Repository.Get(x => x.IdCandidato == IdCandidato && x.Id < IdUltimaCandidatura,
                includeProperties: "IdVagaNavigation,IdVagaNavigation.IdCargoNavigation,IdVagaNavigation.IdCargoNavigation,IdVagaNavigation.IdProcessoSeletivoNavigation.IdEmpresaNavigation")
                                    .OrderByDescending(x => x.Id)

                .Take(5)
                   .ToArray();

        }

        public Candidatura[] ListarPorCandidatoRecentes(int IdCandidato, int IdPrimeiraCandidatura)
        {

            return Repository.Get(x => x.IdCandidato == IdCandidato && x.Id > IdPrimeiraCandidatura,
                includeProperties: "IdVagaNavigation,IdVagaNavigation.IdCargoNavigation,IdVagaNavigation.IdCargoNavigation,IdVagaNavigation.IdProcessoSeletivoNavigation.IdEmpresaNavigation")
                    .OrderByDescending(x => x.Id)

                   .Take(5)
                   .ToArray();

        }


        public bool Salvar(Candidatura Dados)
        {

            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(Candidatura Dados)
        {

            bool resultado = Repository.Update(Dados);

            return resultado;

        }


        public bool ExisteCandidatura(int idVaga, int idCandidato)
        {
            var candidatura = Repository
                .Get(x => x.IdVaga == idVaga && x.IdCandidato == idCandidato)
                .FirstOrDefault();

            return candidatura != null;



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
