using BaseProject.DAO.Context;
using BaseProject.DAO.Interface.Repository;
using BaseProject.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.DAO.Repository
{
    public class RepositoryMachine : Repository<Machine, GlobalContext>, IRepositoryMachine
    {
        public RepositoryMachine() : base() { }

        public RepositoryMachine(GlobalContext context) : base(context) { }

    }
}
