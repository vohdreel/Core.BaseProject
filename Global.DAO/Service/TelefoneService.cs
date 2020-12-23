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
    public class TelefoneService : IDisposable
    {
        private TelefoneRepository Repository { get; set; }

        public TelefoneService()
        {

            Repository = new TelefoneRepository();

        }

        public TelefoneService(GlobalContext context)
        {
            Repository = new TelefoneRepository(context);
        }


        public Telefone Buscar(int Id) 
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public bool Salvar(Telefone Dados)
        {
            
            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(Telefone Dados)
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
