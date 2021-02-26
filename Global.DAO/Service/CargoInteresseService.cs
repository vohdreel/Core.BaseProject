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

        public bool SalvarVarios(CargoInteresse[] Dados)
        {

            bool resultado = Repository.InsertAll(Dados);
            return resultado;

        }

        public CargoInteresse BuscarCargoPorCandidato(int idCandidato, int idCargo)
        {

            return Repository.Get(x => x.IdCandidato == idCandidato && x.IdCargo == idCargo).FirstOrDefault();

        }


        public bool AtualizarListaDeCargosInteressePorCandidato(int idCandidato, int[] idsCargoInteresse)
        {
            CargoInteresse[] cargosRemovidos = BuscarTodosPorCandidato(idCandidato)
                .Where(x => !idsCargoInteresse.Contains(x.IdCargo)).ToArray();
            List<CargoInteresse> cargosNovos = new List<CargoInteresse>();
            foreach (int idCargo in idsCargoInteresse)
            {
                CargoInteresse cargo = BuscarCargoPorCandidato(idCandidato, idCargo);
                if (cargo == null)
                {
                    cargosNovos.Add(new CargoInteresse()
                    {
                        IdCandidato = idCandidato,
                        IdCargo = idCargo

                    });

                }

            }
            bool sucesso = ExcluirVarios(cargosRemovidos);
            if (sucesso) sucesso = SalvarVarios(cargosNovos.ToArray());

            return sucesso;


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
