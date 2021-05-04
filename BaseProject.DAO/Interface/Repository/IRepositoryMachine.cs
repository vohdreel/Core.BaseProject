using BaseProject.DAO.Context;
using BaseProject.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.DAO.Interface.Repository
{
    public interface IRepositoryMachine: IRepository<Machine, GlobalContext>
    {
    }
}
