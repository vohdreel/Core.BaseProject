using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class Repository : GenericRepository<Candidato, GlobalContext>
    {

        public Repository() :base(){ }

        public Repository(GlobalContext context) : base(context) { }
    }
}