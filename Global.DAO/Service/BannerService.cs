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
    public class BannerService : IDisposable
    {
        private BannerRepository Repository { get; set; }

        public BannerService()
        {

            Repository = new BannerRepository();

        }

        public BannerService(GlobalContext context)
        {
            Repository = new BannerRepository(context);
        }


        public Banner Buscar(int Id)
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public Banner[] BuscarTodos()
        {

            return Repository.Get().ToArray();

        }

        public bool Salvar(Banner Dados)
        {

            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(Banner Dados)
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
