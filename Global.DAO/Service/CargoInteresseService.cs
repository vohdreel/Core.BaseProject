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
    public class CargoInteresseService : IDisposable
    {
        private CargoInteresseRepository Repository { get; set; }

        public CargoInteresseService()
        {

            Repository = new CargoInteresseRepository();

        }

        public CargoInteresseService(GlobalContext context)
        {
            Repository = new CargoInteresseRepository(context);
        }


        public CargoInteresse Buscar(int Id) 
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }
        public CargoInteresse[] BuscarTodos() 
        {

            return Repository.Get().ToArray();

        }

        public CargoInteresse[] BuscarTodosPorCandidato(int idCandidato)
        {

            return Repository.Get(x => x.IdCandidato == idCandidato).ToArray();

        }

        public bool Salvar(CargoInteresse Dados)
        {
            
            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(CargoInteresse Dados)
        {

            bool resultado = Repository.Update(Dados);

            return resultado;

        }

        public bool Excluir(int Id)
        {
            var dados = Repository.GetByID(Id);
            bool resultado = Repository.Delete(dados);

            return resultado;
        }

        public bool ExcluirVarios(CargoInteresse[] records)
        {
            bool resultado = Repository.DeleteAll(records);

            return resultado;
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
