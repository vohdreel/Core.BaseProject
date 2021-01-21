using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class TelefoneEmpresaRepository : GenericRepository<TelefoneEmpresa, GlobalContext>
    {

        public TelefoneEmpresaRepository() :base(){ }

        public TelefoneEmpresaRepository(GlobalContext context) : base(context) { }
    }
}