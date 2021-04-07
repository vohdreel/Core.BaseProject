using Global.DAO.Context;
using Global.DAO.Model;
using Global.DAO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Service
{
    public class ExperienciaProfissionalService: IDisposable
    {
        private ExperienciaProfissionalRepository Repository { get; set; }

        public ExperienciaProfissionalService()
        {

            Repository = new ExperienciaProfissionalRepository();

        }

        public ExperienciaProfissionalService(GlobalContext context)
        {
            Repository = new ExperienciaProfissionalRepository(context);
        }


        public ExperienciaProfissional Buscar(int Id)
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public ExperienciaProfissional[] BuscarPorCandidato(int IdCandidato)
        {

            return Repository.Get(x => x.IdCandidato == IdCandidato).ToArray();
        
        
        }

        public bool Salvar(ExperienciaProfissional Dados)
        {

            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(ExperienciaProfissional Dados)
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
