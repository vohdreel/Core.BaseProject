using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class UnidadRepository : GenericRepository<Unidade, GlobalContext>
    {
        public UnidadRepository() : base(new GlobalContext())
        {

        }

        public UnidadRepository(GlobalContext context) : base(context)
        {

        }
    }
}
