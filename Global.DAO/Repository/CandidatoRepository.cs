using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class CandidatoRepository : GenericRepository<Candidato, GlobalContext>
    {

        public CandidatoRepository() :base(){ }

        public CandidatoRepository(GlobalContext context) : base(context) { }
    }
}