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
    public class EmpresaService : IDisposable
    {
        private EmpresaRepository Repository { get; set; }

        public EmpresaService()
        {

            Repository = new EmpresaRepository();

        }

        public EmpresaService(GlobalContext context)
        {
            Repository = new EmpresaRepository(context);
        }


        public Empresa Buscar(int Id) 
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public Empresa BuscarPorNomeFantasia(string NomeFantasia)
        {

            return Repository.Get(x => x.NomeFantasia == NomeFantasia).FirstOrDefault();

        }

        public bool Salvar(Empresa Dados)
        {
            
            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(Empresa Dados)
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
