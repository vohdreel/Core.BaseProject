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
    public class AreaInteresseService : IDisposable
    {
        private AreaInteresseRepository Repository { get; set; }

        public AreaInteresseService()
        {

            Repository = new AreaInteresseRepository();

        }

        public AreaInteresseService(GlobalContext context)
        {
            Repository = new AreaInteresseRepository(context);
        }


        public AreaInteresse Buscar(int Id)
        {

            return Repository.Get(x => x.Id == Id).FirstOrDefault();

        }

        public AreaInteresse[] BuscarTodos()
        {

            return Repository.Get().ToArray();

        }

        public bool Salvar(AreaInteresse Dados)
        {

            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(AreaInteresse Dados)
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

        public AreaInteresse[] BuscarTodosPorCandidato(int idCandidato)
        {

            return Repository.Get(x => x.IdCandidato == idCandidato, includeProperties: "IdEnumAgrupamentoNavigation").ToArray();

        }

        public AreaInteresse BuscarAgrupamentoPorCandidato(int idCandidato, int idEnumAgrupamento)
        {

            return Repository.Get(x => x.IdCandidato == idCandidato && x.IdEnumAgrupamento == idEnumAgrupamento).FirstOrDefault();

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
            bool resultado = Repository.DeleteAll(records);

            return resultado;
        }

        public bool SalvarVarios(AreaInteresse[] Dados)
        {

            bool resultado = Repository.InsertAll(Dados);
            return resultado;

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
