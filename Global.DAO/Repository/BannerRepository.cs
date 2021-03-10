using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class BannerRepository : GenericRepository<Banner, GlobalContext>
    {

        public BannerRepository() : base() { }

        public BannerRepository(GlobalContext context) : base(context) { }
    }
}
