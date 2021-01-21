using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class AreaInteresseRepository : GenericRepository<AreaInteresse, GlobalContext>
    {

        public AreaInteresseRepository() :base(){ }

        public AreaInteresseRepository(GlobalContext context) : base(context) { }
    }
}