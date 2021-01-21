using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class TelefoneRepository : GenericRepository<Telefone, GlobalContext>
    {

        public TelefoneRepository() :base(){ }

        public TelefoneRepository(GlobalContext context) : base(context) { }
    }
}