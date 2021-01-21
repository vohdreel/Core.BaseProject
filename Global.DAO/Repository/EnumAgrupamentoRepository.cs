using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class EnumAgrupamentoRepository : GenericRepository<EnumAgrupamento, GlobalContext>
    {

        public EnumAgrupamentoRepository() :base(){ }

        public EnumAgrupamentoRepository(GlobalContext context) : base(context) { }
    }
}