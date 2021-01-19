using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Interface.Service
{
    interface IServiceDocumento
    {
        IEnumerable<Documento> Listar();
        bool Criar(Documento documento);
        bool Atualizar(Documento documento);
        bool Deletar(int idDocumento);
        Documento Get(int idDocumento);
        void Finalizar(int id);
    }
}
