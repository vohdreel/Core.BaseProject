using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class CargoRepository : GenericRepository<Cargo, GlobalContext>
    {

        public CargoRepository() :base(){ }

        public CargoRepository(GlobalContext context) : base(context) { }
    }
}