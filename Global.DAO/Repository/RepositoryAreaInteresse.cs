using Global.DAO.Context;
using Global.DAO.Interface.Repository;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class RepositoryAreaInteresse : Repository<AreaInteresse, GlobalContext>, IRepositoryAreaInteresse
    {

        public RepositoryAreaInteresse() :base(){ }

        public RepositoryAreaInteresse(GlobalContext context) : base(context) { }
    }
}