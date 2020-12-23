using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class CandidaturaRepository : GenericRepository<Candidatura, GlobalContext>
    {

        public CandidaturaRepository() :base(){ }

        public CandidaturaRepository(GlobalContext context) : base(context) { }
    }
}