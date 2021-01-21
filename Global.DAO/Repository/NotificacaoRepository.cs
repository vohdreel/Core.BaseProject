using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class NotificacaoRepository : GenericRepository<Notificacao, GlobalContext>
    {

        public NotificacaoRepository() :base(){ }

        public NotificacaoRepository(GlobalContext context) : base(context) { }
    }
}