using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Interface.Service
{
    public interface IService<TEntity>
    {
        IEnumerable<TEntity> Listar();
        //CREATE
        bool Salvar(TEntity entity);
        //READ
        TEntity BuscarPorId(int entityId);
        //UPDATE
        bool Atualizar(TEntity entity);
        //DELETE
        bool Excluir(int entityId);

    }
}
