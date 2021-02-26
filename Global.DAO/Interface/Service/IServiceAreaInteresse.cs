using Global.DAO.Interface.Service;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Interface.Repository
{
    public interface IServiceAreaInteresse: IService<AreaInteresse>
    {
        public IEnumerable<AreaInteresse> BuscarTodosPorCandidato(int idCandidato);
        public AreaInteresse BuscarAgrupamentoPorCandidato(int idCandidato, int idEnumAgrupamento);
        public bool AtualizarListaDeAreasInteressePorCandidato(int idCandidato, int[] idsEnumAgrupamento);
        public bool ExcluirVarios(AreaInteresse[] records);
        public bool SalvarVarios(AreaInteresse[] Dados);

    }
}
