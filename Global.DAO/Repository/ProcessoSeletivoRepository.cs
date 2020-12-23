using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class ProcessoSeletivoRepository : GenericRepository<ProcessoSeletivo, GlobalContext>
    {

        public ProcessoSeletivoRepository() :base(){ }

        public ProcessoSeletivoRepository(GlobalContext context) : base(context) { }
    }
}