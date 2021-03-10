using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class ExperienciaProfissionalRepository : GenericRepository<ExperienciaProfissional, GlobalContext>
    {

        public ExperienciaProfissionalRepository() : base() { }

        public ExperienciaProfissionalRepository(GlobalContext context) : base(context) { }
    }
}
