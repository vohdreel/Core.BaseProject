using Global.DAO.Context;
using Global.DAO.Model;
using Global.DAO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Service
{
    public class LogCandidatoService : IDisposable
    {
        private LogCandidatoRepository Repository { get; set; }

        public LogCandidatoService()
        {

            Repository = new LogCandidatoRepository();

        }

        public LogCandidatoService(GlobalContext context)
        {
            Repository = new LogCandidatoRepository(context);
        }


        public LogCandidato Buscar(int Id)
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public bool ExisteIdLegado(string IdLegado)
        {

            return Repository.Get(x => x.IdLegado == IdLegado).FirstOrDefault() == null;

        }

        public LogCandidato[] BuscarTodos()
        {

            return Repository.Get().ToArray();

        }

        public bool Salvar(LogCandidato Dados)
        {

            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(LogCandidato Dados)
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
