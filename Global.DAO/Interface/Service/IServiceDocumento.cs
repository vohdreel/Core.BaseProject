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
        bool Excluir(int idDocumento);
        Documento BuscarPorId(int idDocumento);
    }
}
