using BaseProject.DAO.Interface.Repository;
using BaseProject.DAO.Interface.Service;
using BaseProject.DAO.Model;
using BaseProject.DAO.Procedure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.DAO.Service
{
    public class ServiceMachine : IServiceMachine
    {
        private readonly IRepositoryMachine RepositoryMachine;

        public ServiceMachine(IRepositoryMachine repositoryMachine)
        {

            RepositoryMachine = repositoryMachine;

        }

        public Machine BuscarPorId(int Id)
        {
            return RepositoryMachine.Get(x => x.Id == Id).FirstOrDefault();
        }

        public Machine[] ListarPorUsuario(int idUser)
        {
            return RepositoryMachine.Get(x => x.IdUser == idUser).ToArray();

        }

        public bool Salvar(Machine Dados)
        {

            return RepositoryMachine.Insert(Dados);
        }

        public bool Editar(Machine Dados)
        {

            return RepositoryMachine.Update(Dados);

        }

        public bool Excluir(int Id)
        {
            bool resultado = RepositoryMachine.Delete(Id);

            return resultado;
        }

        public IEnumerable<Machine> Listar()
        {

            return RepositoryMachine.Get();
        }

        public BattleUnit[] ListarBattleUnits()
        {
            return RepositoryMachine.
                GetContext()
                .Set<BattleUnit>()
                .FromSqlInterpolated($"EXEC [SelectAllBattleUnits]")
                .AsEnumerable()
                .ToArray();

        }
    }
}
