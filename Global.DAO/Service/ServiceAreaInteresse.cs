using Global.DAO.Context;
using Global.DAO.Interface.Repository;
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
    public class ServiceAreaInteresse : IServiceAreaInteresse
    {

        private readonly IRepositoryAreaInteresse RepositoryAreaInteresse;

        public ServiceAreaInteresse(IRepositoryAreaInteresse repositoryAreaInteresse)
        {

            RepositoryAreaInteresse = repositoryAreaInteresse;

        }

        public AreaInteresse BuscarPorId(int Id)
        {

            return RepositoryAreaInteresse.Get(x => x.Id == Id).FirstOrDefault();

        }

        public IEnumerable<AreaInteresse> Listar()
        {

            return RepositoryAreaInteresse.Get().ToArray();

        }

        public bool Salvar(AreaInteresse Dados)
        {

            bool resultado = RepositoryAreaInteresse.Insert(Dados);
            return resultado;

        }

        public bool Atualizar(AreaInteresse Dados)
        {

            bool resultado = RepositoryAreaInteresse.Update(Dados);

            return resultado;

        }


        public bool Excluir(int Id)
        {
            bool resultado = RepositoryAreaInteresse.Delete(Id);

            return resultado;
        }

        public IEnumerable<AreaInteresse> BuscarTodosPorCandidato(int idCandidato)
        {

            return RepositoryAreaInteresse.Get(x => x.IdCandidato == idCandidato).ToArray();
           // return Repository.Get(x => x.IdCandidato == idCandidato, includeProperties: "IdEnumAgrupamentoNavigation").ToArray();

        }

        public AreaInteresse BuscarAgrupamentoPorCandidato(int idCandidato, int idEnumAgrupamento)
        {

            return RepositoryAreaInteresse.Get(x => x.IdCandidato == idCandidato && x.IdEnumAgrupamento == idEnumAgrupamento).FirstOrDefault();

        }



        public bool AtualizarListaDeAreasInteressePorCandidato(int idCandidato, int[] idsEnumAgrupamento)
        {
            AreaInteresse[] cargosRemovidos = BuscarTodosPorCandidato(idCandidato)
                .Where(x => !idsEnumAgrupamento.Contains(x.IdEnumAgrupamento)).ToArray();
            List<AreaInteresse> cargosNovos = new List<AreaInteresse>();
            foreach (int idEnumAgrupamento in idsEnumAgrupamento)
            {
                AreaInteresse cargo = BuscarAgrupamentoPorCandidato(idCandidato, idEnumAgrupamento);
                if (cargo == null)
                {
                    cargosNovos.Add(new AreaInteresse()
                    {
                        IdCandidato = idCandidato,
                        IdEnumAgrupamento = idEnumAgrupamento

                    });

                }

            }
            bool sucesso = ExcluirVarios(cargosRemovidos);
            if (sucesso) sucesso = SalvarVarios(cargosNovos.ToArray());

            return sucesso;


        }

        public bool ExcluirVarios(AreaInteresse[] records)
        {
            bool resultado = RepositoryAreaInteresse.DeleteAll(records);

            return resultado;
        }

        public bool SalvarVarios(AreaInteresse[] Dados)
        {

            bool resultado = RepositoryAreaInteresse.InsertAll(Dados);
            return resultado;

        }
    }
}
