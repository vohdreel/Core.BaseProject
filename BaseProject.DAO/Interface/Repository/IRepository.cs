using BaseProject.DAO.Context;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BaseProject.DAO.Interface.Repository
{
    public interface IRepository<TEntity, TContext>
    where TEntity : class
    where TContext : IdentityDbContext, new()
    {

        bool Insert(TEntity entity);

        bool InsertAll(IEnumerable<TEntity> entities);

        bool Delete(int IdEntity);

        bool DeleteAll(IEnumerable<TEntity> entitiesToDelete);

        bool Update(TEntity entityToUpdate);

        bool UpdateAll(IEnumerable<TEntity> entities);

        TEntity[] Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            bool noTracking = false);

        TContext GetContext();


    }
}
