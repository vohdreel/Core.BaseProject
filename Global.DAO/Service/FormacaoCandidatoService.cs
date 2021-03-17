using Global.DAO.Context;
using Global.DAO.Model;
using Global.DAO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Service
{
    public class FormacaoCandidatoService : IDisposable
    {
        private FormacaoCandidatoRepository Repository { get; set; }

        public FormacaoCandidatoService()
        {

            Repository = new FormacaoCandidatoRepository();

        }

        public FormacaoCandidatoService(GlobalContext context)
        {
            Repository = new FormacaoCandidatoRepository(context);
        }


        public FormacaoCandidato Buscar(int Id)
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public FormacaoCandidato[] BuscarPorCandidato(int IdCandidato)
        {

            return Repository.Get(x => x.IdCandidato == IdCandidato).ToArray();


        }

        public bool Salvar(FormacaoCandidato Dados)
        {

            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(FormacaoCandidato Dados)
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
