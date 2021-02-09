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
    public class AreaInteresseService : IDisposable
    {
        private AreaInteresseRepository Repository { get; set; }

        public AreaInteresseService()
        {

            Repository = new AreaInteresseRepository();

        }

        public AreaInteresseService(GlobalContext context)
        {
            Repository = new AreaInteresseRepository(context);
        }


        public AreaInteresse Buscar(int Id) 
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public AreaInteresse[] BuscarTodos()
        {

            return Repository.Get().ToArray();

        }

        public bool Salvar(AreaInteresse Dados)
        {
            
            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(AreaInteresse Dados)
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
