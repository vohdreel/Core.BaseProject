using Global.DAO.Context;
using Global.DAO.Model;
using Global.DAO.Procedure.Models;
using Global.DAO.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Service
{
    public class MachineService : IDisposable
    {
        private MachineRepository Repository { get; set; }

        public MachineService()
        {

            Repository = new MachineRepository();

        }

        public MachineService(GlobalContext context)
        {
            Repository = new MachineRepository(context);
        }

        public Machine[] GetMachines() 
        {
            return Repository.Get(null, null, includeProperties: "IdUserNavigation").ToArray();
        }
        public MachineUser[] GetMachinesByProcudeure()
        {
            return Repository
                .GetContext()
                .Set<MachineUser>()
                .FromSqlInterpolated($"EXEC [SelectAllMachinesAndUsers]")
                .ToArray();           
        }



        public GlobalContext GetContext()
        {
            return Repository.GetContext();
        }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
}
