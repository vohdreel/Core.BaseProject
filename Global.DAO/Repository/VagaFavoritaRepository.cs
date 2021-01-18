using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class VagaFavoritaRepository : GenericRepository<VagaFavorita, GlobalContext>
    {

        public VagaFavoritaRepository() : base() { }

        public VagaFavoritaRepository(GlobalContext context) : base(context) { }
    }
}