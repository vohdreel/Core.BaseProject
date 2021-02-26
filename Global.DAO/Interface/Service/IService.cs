using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Interface.Service
{
    public interface IService<TEntity>
    {
        IEnumerable<TEntity> Listar();
        bool Salvar(TEntity entity);
        bool Editar(TEntity entity);
        bool Excluir(int entityId);
        TEntity BuscarPorId(int entityId);
        //Documento[] ListarPorCandidato(int idCandidato);
    }
}
