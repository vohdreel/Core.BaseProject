using Global.DAO.Context;
using Global.DAO.Model;
using Global.DAO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Service
{
    public class UnidadeService : IDisposable
    {
        private UnidadRepository Repository { get; set; }

        public UnidadeService()
        {

            Repository = new UnidadRepository();

        }

        public UnidadeService(GlobalContext context)
        {
            Repository = new UnidadRepository(context);


        }


        public Unidade[] Todos()
        {
            return Repository.Get();
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
