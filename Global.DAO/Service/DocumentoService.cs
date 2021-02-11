using Global.DAO.Context;
using Global.DAO.Interface.Repository;
using Global.DAO.Interface.Service;
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
    public class DocumentoService : IServiceDocumento
    {
        private readonly IRepositoryDocumento RepositoryDocumento;

        public DocumentoService(IRepositoryDocumento repositoryDocumento)
        {

            RepositoryDocumento = repositoryDocumento;

        }

        public Documento BuscarPorId(int Id) 
        {
            return RepositoryDocumento.Get(x => x.Id == Id).FirstOrDefault();
        }

        public bool Criar(Documento Dados)
        {
            
            return RepositoryDocumento.Insert(Dados);
        }

        public bool Atualizar(Documento Dados)
        {

            return RepositoryDocumento.Update(Dados);

        }

        public bool Excluir(int Id)
        {
            bool resultado = RepositoryDocumento.Delete(Id);

            return resultado;
        }

        public IEnumerable<Documento> Listar() {

            return RepositoryDocumento.Get();
        }

    }
}
