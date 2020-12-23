using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class CargoInteresseRepository : GenericRepository<CargoInteresse, GlobalContext>
    {

        public CargoInteresseRepository() :base(){ }

        public CargoInteresseRepository(GlobalContext context) : base(context) { }
    }
}