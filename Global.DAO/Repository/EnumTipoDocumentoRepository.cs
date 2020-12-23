using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class EnumTipoDocumentoRepository : GenericRepository<EnumTipoDocumento, GlobalContext>
    {

        public EnumTipoDocumentoRepository() :base(){ }

        public EnumTipoDocumentoRepository(GlobalContext context) : base(context) { }
    }
}