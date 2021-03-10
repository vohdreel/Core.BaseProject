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
    public class EnumAgrupamentoService : IDisposable
    {
        private EnumAgrupamentoRepository Repository { get; set; }

        public EnumAgrupamentoService()
        {

            Repository = new EnumAgrupamentoRepository();

        }

        public EnumAgrupamentoService(GlobalContext context)
        {
            Repository = new EnumAgrupamentoRepository(context);
        }


        public EnumAgrupamento Buscar(int Id) 
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public EnumAgrupamento BuscarPorNome(string nome)
        {

            return Repository.Get(x => x.NomeAgrupamento == nome).FirstOrDefault();

        }

        public EnumAgrupamento[] BuscarTodos()
        {

            return Repository.Get().ToArray();

        }

        public bool Salvar(EnumAgrupamento Dados)
        {
            
            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(EnumAgrupamento Dados)
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

        public EnumAgrupamento BuscarPorNome(string nome)
        {

            return Repository.Get(x => x.NomeAgrupamento == nome).FirstOrDefault();

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
