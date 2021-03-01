using Global.DAO.Context;
using Global.DAO.Interface.Repository;
using Global.DAO.Model;
using Global.DAO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Repository
{
    public class RepositoryDocumento : Repository<Documento, GlobalContext>, IRepositoryDocumento
    {

        public RepositoryDocumento() :base(){ }

        public RepositoryDocumento(GlobalContext context) : base(context) { }
    }
}