using Global.DAO.Context;
using Global.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class DocumentoRepository : GenericRepository<Documento, GlobalContext>
    {

        public DocumentoRepository() :base(){ }

        public DocumentoRepository(GlobalContext context) : base(context) { }
    }
}