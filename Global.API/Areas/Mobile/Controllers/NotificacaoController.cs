using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.DAO.Model;
using Global.DAO.Service;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    public class NotificacaoController : ControllerBase
    {
        public Notificacao[] GetNotificacoes(int IdCandidato, int contador)
        {
            using (var service = new NotificacaoService())
            {
                var notificacoes = service.ObterNotificacoes(IdCandidato, contador);

                return notificacoes;
            }

        }

        public Notificacao[] GetNotificacoesAntigas(int IdCandidato, int IdUltimaNotificacao)
        {
            using (var service = new NotificacaoService())
            {
                var notificacoes = service.ObterNotificacoesAntigas(IdCandidato, IdUltimaNotificacao);

                return notificacoes;
            }

        }

        public Notificacao[] GetNotificacoesRecentes(int IdCandidato, int IdUltimaNotificacao)
        {
            using (var service = new NotificacaoService())
            {
                var notificacoes = service.ObterNotificacoesRecentes(IdCandidato, IdUltimaNotificacao);

                return notificacoes;
            }

        }
    }
}
