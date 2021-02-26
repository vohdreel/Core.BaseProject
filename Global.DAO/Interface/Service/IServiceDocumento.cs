using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Interface.Service
{
    public interface IServiceDocumento
    {
        IEnumerable<Documento> Listar();
        bool Salvar(Documento Dados);
        bool Editar(Documento Dados);
        bool Excluir(int idDocumento);
        Documento BuscarPorId(int idDocumento);
        Documento[] ListarPorCandidato(int idCandidato);

    }
}
