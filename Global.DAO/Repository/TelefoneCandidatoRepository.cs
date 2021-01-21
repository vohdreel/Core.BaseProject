using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class TelefoneCandidatoRepository : GenericRepository<TelefoneCandidato, GlobalContext>
    {

        public TelefoneCandidatoRepository() :base(){ }

        public TelefoneCandidatoRepository(GlobalContext context) : base(context) { }
    }
}