using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class LoggerRepository : GenericRepository<Logger, GlobalContext>
    {

        public LoggerRepository() : base() { }

        public LoggerRepository(GlobalContext context) : base(context) { }
    }
}
