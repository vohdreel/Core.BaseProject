using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class RepositoryCandidato : GenericRepository<Candidato, GlobalContext>
    {

        public RepositoryCandidato() :base(){ }

        public RepositoryCandidato(GlobalContext context) : base(context) { }
    }
}