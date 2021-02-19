using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.DAO.Model;
using Global.DAO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class NotificacaoController : ControllerBase
    {
        [HttpGet("GetNotificacoes")]

        public ViewModel.Notificacao[] GetNotificacoes(int IdCandidato, int contador)
        {
            using (var service = new NotificacaoService())
            {
                var notificacoes = service
                    .ObterNotificacoes(IdCandidato, contador)
                    .Select(x => new ViewModel.Notificacao
                    {

                        IdNotificacao = x.Id,
                        Corpo = x.CorpoNotificacao,
                        Titulo = x.TituloNotificacao,
                        QueryParams = x.QueryParams,
                        IdCandidato = x.IdCandidato,
                        Rota = x.AngularRoute,
                        DataNotificacao = TimeZoneInfo
                        .ConvertTimeFromUtc(x.DataCriacaoNotificacao, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
                        .ToString("dd/MM/yyyy HH:mm tt")

                    }).ToArray();

                return notificacoes;
            }

        }
        [HttpGet("GetNotificacoesAntigas")]
        public ViewModel.Notificacao[] GetNotificacoesAntigas(int IdCandidato, int IdUltimaNotificacao)
        {
            using (var service = new NotificacaoService())
            {
                var notificacoes = service
                    .ObterNotificacoesAntigas(IdCandidato, IdUltimaNotificacao)
                    .Select(x => new ViewModel.Notificacao
                    {

                        IdNotificacao = x.Id,
                        Corpo = x.CorpoNotificacao,
                        Titulo = x.TituloNotificacao,
                        QueryParams = x.QueryParams,
                        IdCandidato = x.IdCandidato,
                        Rota = x.AngularRoute,
                        DataNotificacao = TimeZoneInfo
                        .ConvertTimeFromUtc(x.DataCriacaoNotificacao, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
                        .ToString("dd/MM/yyyy HH:mm tt")

                    }).ToArray();

                return notificacoes;
            }

        }

        [HttpGet("GetNotificacoesRecentes")]
        public ViewModel.Notificacao[] GetNotificacoesRecentes(int IdCandidato, int IdUltimaNotificacao)
        {
            using (var service = new NotificacaoService())
            {
                var notificacoes = service
                    .ObterNotificacoesRecentes(IdCandidato, IdUltimaNotificacao).
                    Select(x => new ViewModel.Notificacao
                    {

                        IdNotificacao = x.Id,
                        Corpo = x.CorpoNotificacao,
                        Titulo = x.TituloNotificacao,
                        QueryParams = x.QueryParams,
                        IdCandidato = x.IdCandidato,
                        Rota = x.AngularRoute,
                        DataNotificacao = TimeZoneInfo
                        .ConvertTimeFromUtc(x.DataCriacaoNotificacao, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
                        .ToString("dd/MM/yyyy HH:mm tt")

                    }).ToArray();

                return notificacoes;
            }

        }
    }
}
