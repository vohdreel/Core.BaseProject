using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.DAO.Model;
using Global.DAO.Service;
using Global.Util.SystemEnumerations;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    public class VagaController : ControllerBase
    {
        public ViewModel.Vaga[] GetVagasGeraisPaginaInicial()
        {
            using (var service = new VagaService())
            {
                var vagas = service.BuscarVagasGerais().Select(x => new ViewModel.Vaga
                {
                    IdVaga = x.Id,
                    NomeCargo = x.IdCargoNavigation.NomeCargo,
                    NomeEmpresa = x.IdProcessoSeletivoNavigation.IdEmpresaNavigation.NomeFantasia,
                    Salario = x.Salario?.ToString("c"),
                    Modalidade = ((VagaModalidade)x.Modalidade).ToString(),
                    Cidade = x.Cidade,
                    Estado = x.Estado,
                    Requisitos = x.Requisitos,
                    Beneficios = x.Beneficios

                }).ToArray();
                return vagas;                                
            
            }
        }
    }
}
