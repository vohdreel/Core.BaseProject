using Global.DAO.Context;
using Global.DAO.Model;
using Global.DAO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Service
{
    public class LoggerService : IDisposable
    {
        private LoggerRepository Repository { get; set; }

        public LoggerService()
        {

            Repository = new LoggerRepository();

        }

        public LoggerService(GlobalContext context)
        {
            Repository = new LoggerRepository(context);
        }


        public Logger Buscar(int Id)
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public Logger[] BuscarTodos()
        {

            return Repository.Get().ToArray();

        }

        public bool Salvar(Logger Dados)
        {

            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(Logger Dados)
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
