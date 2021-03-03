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
    public class CargoService : IDisposable
    {
        private CargoRepository Repository { get; set; }

        public CargoService()
        {

            Repository = new CargoRepository();

        }

        public CargoService(GlobalContext context)
        {
            Repository = new CargoRepository(context);
        }


        public Cargo Buscar(int Id) 
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public Cargo[] BuscarTodos()
        {

            return Repository.Get().ToArray();

        }

        public bool Salvar(Cargo Dados)
        {
            
            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(Cargo Dados)
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

        /// <summary>
        /// Verifica a existencia de uma vaga encontrada no Feed do Sistema IHunter na base da Global
        /// </summary>
        /// <remarks>
        /// Essas vagas sempre serão comparadas pelo nome e pelo nome e pelo enumAgrupamento fixo "Prestação de Serviços"
        /// </remarks>
        public Cargo BuscarCargoFeed(string nomeCargo)
        {
            return Repository.Get(x => x.NomeCargo == nomeCargo && x.IdEnumAgrupamentoNavigation.NomeAgrupamento == "Prestação de Serviços", includeProperties: "IdEnumAgrupamentoNavigation")
                .FirstOrDefault();
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
