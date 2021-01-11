using Global.DAO.Context;
using Global.DAO.Model;
using Global.DAO.Procedure.Models;
using Global.DAO.Repository;
using Global.Util;
using Global.Util.SystemEnumerations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Service
{
    public class VagaService : IDisposable
    {
        private VagaRepository Repository { get; set; }

        public VagaService()
        {

            Repository = new VagaRepository();

        }

        public VagaService(GlobalContext context)
        {
            Repository = new VagaRepository(context);
        }


        public Vaga Buscar(int Id)
        {

            return Repository
                .Get(x => x.Id == Id, includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation")
                .FirstOrDefault();

        }


        public Vaga[] BuscarVagasGerais()
        {

            return Repository
                .Get(includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation")
                .OrderByDescending(x => x.Id)
                .ThenByDescending(x => x.IdProcessoSeletivoNavigation.DataInicioProcesso)
                .Take(5)
                .ToArray();

        }

        public Vaga[] BuscarVagasGeraisAntigas(int idUltimaVaga)
        {
            return Repository
                    .Get(x => x.Id < idUltimaVaga, includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation")
                    .OrderByDescending(x => x.Id)
                .ThenByDescending(x => x.IdProcessoSeletivoNavigation.DataInicioProcesso)
                .Take(5)
                    .ToArray();


        }

        public Vaga[] BuscarVagasGeraisRecentes(int idPrimeiraVaga)
        {
            return Repository
                    .Get(x => x.Id > idPrimeiraVaga, includeProperties: "IdProcessoSeletivoNavigation,IdProcessoSeletivoNavigation.IdEmpresaNavigation,IdCargoNavigation")
                   .OrderByDescending(x => x.Id)
                .ThenByDescending(x => x.IdProcessoSeletivoNavigation.DataInicioProcesso)
                .Take(5)
                    .ToArray();


        }


        public bool Salvar(Vaga Dados)
        {

            bool resultado = Repository.Insert(Dados);
            return resultado;

        }

        public bool Editar(Vaga Dados)
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
