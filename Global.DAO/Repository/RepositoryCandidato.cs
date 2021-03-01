using Global.DAO.Context;
using Global.DAO.Interface.Repository;
using Global.DAO.Model;
using Global.DAO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class RepositoryCandidato : Repository<Candidato, GlobalContext>, IRepositoryCandidato
    {

        public RepositoryCandidato() :base(){ }

        public RepositoryCandidato(GlobalContext context) : base(context) { }
    }
}