using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class MachineRepository : GenericRepository<Machine, GlobalContext>
    {

        public MachineRepository() :base(){ }

        public MachineRepository(GlobalContext context) : base(context) { }
    }
}
