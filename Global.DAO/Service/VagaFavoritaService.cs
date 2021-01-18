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
    public class VagaFavoritaService : IDisposable
    {
        private VagaFavoritaRepository Repository { get; set; }

        public VagaFavoritaService()
        {

            Repository = new VagaFavoritaRepository();

        }

        public VagaFavoritaService(GlobalContext context)
        {
            Repository = new VagaFavoritaRepository(context);
        }


        public VagaFavorita Buscar(int Id)
        {

            return Repository
                .Get(x => x.Id == Id, includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation")
                .FirstOrDefault();

        }

        public Vaga[] BuscarVagasFavoritasCandidato(int IdCandidato)
        {
            return Repository 
                .Get(x => x.IdCandidato == IdCandidato, includeProperties: "IdVagaNavigation,IdVagaNavigation.IdCargoNavigation,IdVagaNavigation.IdProcessoSeletivoNavigation,IdVagaNavigation.IdProcessoSeletivoNavigation.IdEmpresaNavigation").Select(x=>x.IdVagaNavigation).ToArray();
        }


        //public VagaFavorita[] BuscarVagaFavoritasGerais()
        //{
        //    return Repository
        //        .Get(includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation")
        //        .OrderByDescending(x => x.Id)
        //        .ThenByDescending(x => x.IdProcessoSeletivoNavigation.DataInicioProcesso)
        //        .Take(5)
        //        .ToArray();
        //}

        public bool Salvar(VagaFavorita Dados)
        {
            bool resultado = Repository.Insert(Dados);
            return resultado;
        }

        public bool Editar(VagaFavorita Dados)
        {

            bool resultado = Repository.Update(Dados);

            return resultado;

        }

        public bool Excluir(int Id)
        {
            var dados = Repository.GetByID(Id);
            bool resultado = Repository.Delete(dados);

            return resultado;
        }

        public bool ExcluirPorIdCandidatoIdVaga(int IdCanditado, int IdVaga)
        {
            var dados = Repository.Get(x => x.IdCandidato == IdCanditado && x.IdVaga == IdVaga);
            bool resultado = Repository.DeleteAll(dados);

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
