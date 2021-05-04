using BaseProject.DAO.Model;
using BaseProject.DAO.Procedure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.DAO.Interface.Service
{
    public interface IServiceMachine
    {
        IEnumerable<Machine> Listar();
        bool Salvar(Machine Dados);
        bool Editar(Machine Dados);
        bool Excluir(int idDocumento);
        Machine BuscarPorId(int idDocumento);
        Machine[] ListarPorUsuario(int idCandidato);
        BattleUnit[] ListarBattleUnits();
    }
}
