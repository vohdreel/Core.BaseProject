using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class EmpresaRepository : GenericRepository<Empresa, GlobalContext>
    {

        public EmpresaRepository() :base(){ }

        public EmpresaRepository(GlobalContext context) : base(context) { }
    }
}