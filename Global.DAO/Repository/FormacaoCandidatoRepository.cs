using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class FormacaoCandidatoRepository : GenericRepository<FormacaoCandidato, GlobalContext>
    {

        public FormacaoCandidatoRepository() : base() { }

        public FormacaoCandidatoRepository(GlobalContext context) : base(context) { }
    }
}
