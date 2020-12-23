using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class VagaRepository : GenericRepository<Vaga, GlobalContext>
    {

        public VagaRepository() :base(){ }

        public VagaRepository(GlobalContext context) : base(context) { }
    }
}