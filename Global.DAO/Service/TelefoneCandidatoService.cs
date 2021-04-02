using Global.DAO.Context;
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
    public class TelefoneCandidatoService : IDisposable
    {
        private TelefoneCandidatoRepository Repository { get; set; }

        public TelefoneCandidatoService()
        {

            Repository = new TelefoneCandidatoRepository();

        }

        public TelefoneCandidatoService(GlobalContext context)
        {
            Repository = new TelefoneCandidatoRepository(context);
        }


        public TelefoneCandidato Buscar(int Id) 
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public Telefone[] BuscarTelefonesPorCandidato(int IdCandidato)
        {

            return Repository.Get(x => x.IdCandidato == IdCandidato, includeProperties: "IdTelefoneNavigation").Select(x => x.IdTelefoneNavigation).ToArray();

        }

        public bool Salvar(TelefoneCandidato Dados)
        {
            
            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(TelefoneCandidato Dados)
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
