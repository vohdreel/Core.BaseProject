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
    public class TelefoneEmpresaService : IDisposable
    {
        private TelefoneEmpresaRepository Repository { get; set; }

        public TelefoneEmpresaService()
        {

            Repository = new TelefoneEmpresaRepository();

        }

        public TelefoneEmpresaService(GlobalContext context)
        {
            Repository = new TelefoneEmpresaRepository(context);
        }


        public TelefoneEmpresa Buscar(int Id) 
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public bool Salvar(TelefoneEmpresa Dados)
        {
            
            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(TelefoneEmpresa Dados)
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
