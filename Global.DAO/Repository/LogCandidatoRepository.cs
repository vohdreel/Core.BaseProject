using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class LogCandidatoRepository : GenericRepository<LogCandidato, GlobalContext>
    {

        public LogCandidatoRepository() : base() { }

        public LogCandidatoRepository(GlobalContext context) : base(context) { }
    }
}
