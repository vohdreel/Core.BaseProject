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
    public class NotificacaoService : IDisposable
    {
        private NotificacaoRepository Repository { get; set; }

        public NotificacaoService()
        {

            Repository = new NotificacaoRepository();

        }

        public NotificacaoService(GlobalContext context)
        {
            Repository = new NotificacaoRepository(context);
        }

        public Notificacao[] ObterNotificacoes(int IdCandidato, int contador)
        {
            return Repository.Get(x => x.IdCandidato == IdCandidato)
                .OrderByDescending(x => x.DataCriacaoNotificacao)
                .Take(contador)
                .Skip(contador - 10)
                .ToArray();
        
        
        
        }

        public Notificacao[] ObterNotificacoesAntigas(int IdCandidato, int IdUltimaNotificacao)
        {
            return Repository.Get(x => x.IdCandidato == IdCandidato && x.Id < IdUltimaNotificacao)
                .OrderByDescending(x => x.DataCriacaoNotificacao)
                .Take(10)
                .ToArray();
        }

        public Notificacao[] ObterNotificacoesRecentes(int IdCandidato, int IdPrimeiraNotificacao)
        {
            return Repository.Get(x => x.IdCandidato == IdCandidato && x.Id > IdPrimeiraNotificacao )
                .OrderByDescending(x => x.DataCriacaoNotificacao)
                .Take(10)
                .ToArray();
        }


        public bool MarcarNotificacaoComoLida(int IdNotificacao) 
        {
            Notificacao notificacao = Repository.Get(x => x.Id == IdNotificacao).FirstOrDefault();
            if (notificacao != null) notificacao.Visualizado = true;
            return Repository.Update(notificacao);
            
        
        
        }


        public bool SalvarNotificao(Notificacao notificacao)
        {
            return Repository.Insert(notificacao);       

        
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
