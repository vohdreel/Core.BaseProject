using BaseProject.DAO.Interface;
using BaseProject.DAO.Interface.Repository;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BaseProject.DAO.Repository
{
    public abstract class Repository<TEntity, TContext> : IDisposable, IRepository<TEntity>
    where TEntity : class
    where TContext : IdentityDbContext, new()
    {
        internal TContext context;
        internal DbSet<TEntity> set;
        internal int bulkCount;

        public Repository(TContext context)
        {
            this.UpdateContext(context);
        }

        public Repository()
        {
            this.UpdateContext(new TContext());
        }

        private void UpdateContext(TContext context)
        {
            this.context = context;
            this.set = context.Set<TEntity>();
            this.bulkCount = 0;
        }

        protected TContext Context
        {
            get
            {
                return context;
            }
        }

        public DbSet<TEntity> Set
        {
            get
            {
                return this.set;
            }
        }

        public virtual TEntity GetByID(object id)
        {
            TEntity entity = null;

            try
            {
                entity = set.Find(id);
            }
            catch (Exception e) { }

            return entity;
        }


        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            TEntity entity = null;

            try
            {
                entity = await set.FindAsync(id);
            }
            catch (Exception) { }

            return entity;
        }

        public virtual bool Insert(TEntity entity)
        {
            try
            {
                set.Add(entity);
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                context.Dispose();
                return false;
            }
        }
        public virtual bool InsertTrans(TEntity entity)
        {
            try
            {
                set.Add(entity);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            set.Add(entity);

            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public virtual bool InsertAll(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
            {
                return true;
            }

            set.AddRange(entities);

            try
            {
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public virtual bool InsertAllTrans(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
            {
                return true;
            }

            set.AddRange(entities);

            try
            {

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual async Task<bool> InsertAllAsync(IEnumerable<TEntity> entities)
        {
            if (!entities.Any())
            {
                return true;
            }

            set.AddRange(entities);

            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool Delete(int IdEntity)
        {
            TEntity entityToDelete = set.Find(IdEntity);
            return Delete(entityToDelete);
        }

        public virtual async Task<bool> DeleteAsync(object id)
        {
            TEntity entityToDelete = set.Find(id);
            return await DeleteAsync(entityToDelete);
        }

        public virtual bool DeleteByFilter(Expression<Func<TEntity, bool>> filter)
        {
            TEntity[] toDelete = set.Where(filter).ToArray();

            set.RemoveRange(toDelete);

            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public virtual async Task<bool> DeleteByFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            TEntity[] toDelete = set.Where(filter).ToArray();

            set.RemoveRange(toDelete);

            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                set.Attach(entityToDelete);
            }
            set.Remove(entityToDelete);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteAsync(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                set.Attach(entityToDelete);
            }
            set.Remove(entityToDelete);
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// filtro criado para atender a especificação do datatables
        /// </summary>
        /// <param name="initialIndex">Posição começando do zero do primeiro valor da pagina</param>
        /// <param name="ItensPerPage">itens por pagina</param>
        /// <param name="TotalElements">variavel que contera o numero de registro no banco</param>
        /// <param name="TotalFilteredElements">variavel que contera o total de registros filtrados no banco</param>
        /// <param name="filter">função de filtragem</param>        
        /// <param name="orderBy">função de ordenação</param>
        /// <param name="preFilter">filtro usado para controlar a contagem do total</param>
        /// <returns>Todos os registros desta pagina</returns>
        public virtual TEntity[] Filter
        (
            int initialIndex,
            int ItensPerPage,
            out int TotalElements,
            out int TotalFilteredElements,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, bool>> preFilter = null
        )
        {
            IQueryable<TEntity> entities = this.set;

            if (preFilter != null)
            {
                entities = entities.Where(preFilter);
            }

            TotalElements = entities.Count();

            if (filter != null)
            {
                entities = entities.Where(filter);
            }

            if (orderBy != null)
            {
                entities = orderBy(entities);
            }

            var paged = entities;

            TotalFilteredElements = paged.Count();

            paged = paged.Skip(initialIndex);

            if (ItensPerPage > 0)
            {
                paged = paged.Take(ItensPerPage);
            }

            return paged.ToArray();
        }

        public virtual bool DeleteAll(IEnumerable<TEntity> entitiesToDelete)
        {
            if (entitiesToDelete.Count() == 0)
            {
                return true;
            }

            int entityCount = entitiesToDelete.Count();
            for (int i = 0; i < entityCount; i++)
            {

                if (context.Entry(entitiesToDelete.ElementAt(i)).State == EntityState.Detached)
                {
                    set.Attach(entitiesToDelete.ElementAt(i));
                }
            }

            set.RemoveRange(entitiesToDelete);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public virtual bool DeleteAllTrans(IEnumerable<TEntity> entitiesToDelete)
        {
            if (entitiesToDelete.Count() == 0)
            {
                return true;
            }

            int entityCount = entitiesToDelete.Count();
            for (int i = 0; i < entityCount; i++)
            {

                if (context.Entry(entitiesToDelete.ElementAt(i)).State == EntityState.Detached)
                {
                    set.Attach(entitiesToDelete.ElementAt(i));
                }
            }

            set.RemoveRange(entitiesToDelete);
            try
            {
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteAllAsync(IEnumerable<TEntity> entitiesToDelete)
        {
            if (entitiesToDelete.Count() == 0)
            {
                return true;
            }

            int entityCount = entitiesToDelete.Count();
            for (int i = 0; i < entityCount; i++)
            {

                if (context.Entry(entitiesToDelete.ElementAt(i)).State == EntityState.Detached)
                {
                    set.Attach(entitiesToDelete.ElementAt(i));
                }
            }

            set.RemoveRange(entitiesToDelete);
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool Update(TEntity entityToUpdate)
        {
            try
            {

                context.Entry(entityToUpdate).State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException e)
            {
                return false;
            }
            catch (Exception e)
            {

                return false;
            }
        }
        public virtual bool UpdateTrans(TEntity entityToUpdate)
        {
            try
            {
                context.Entry(entityToUpdate).State = EntityState.Modified;
                return true;
            }
            catch (DbEntityValidationException e)
            {
                return false;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public virtual async Task<bool> UpdateAsync(TEntity entityToUpdate)
        {
            try
            {
                context.Entry(entityToUpdate).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public virtual bool UpdateOutsideContext(TEntity entityToUpdate)
        {
            var current = set.Find(entityToUpdate.GetType().GetProperty("Id").GetValue(entityToUpdate, null));
            context.Entry(current).CurrentValues.SetValues(entityToUpdate);
            context.Entry(current).State = EntityState.Modified;
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool UpdateAll(IEnumerable<TEntity> entities)
        {
            int entitiesCount = entities.Count();

            if (entitiesCount == 0) return true;

            for (int i = 0; i < entitiesCount; i++)
            {
                try
                {
                    context.Entry(entities.ElementAt(i)).State = EntityState.Modified;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual async Task<bool> UpdateAllAsync(IEnumerable<TEntity> entities)
        {
            int entitiesCount = entities.Count();

            if (entitiesCount == 0) return true;

            for (int i = 0; i < entitiesCount; i++)
            {
                context.Entry(entities.ElementAt(i)).State = EntityState.Modified;
            }
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool UpdateAllOutsideContext(IEnumerable<TEntity> entities)
        {
            int entitiesCount = entities.Count();

            for (int i = 0; i < entitiesCount; i++)
            {
                var current = set.Find(entities.ElementAt(i).GetType().GetProperty("Id").GetValue(entities.ElementAt(i), null));
                context.Entry(current).CurrentValues.SetValues(entities.ElementAt(i));
                context.Entry(current).State = EntityState.Modified;
            }

            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private PropertyInfo getPropertyId(TEntity entity)
        {
            var properties = entity.GetType().GetProperties()
                                .Where(property => Regex.IsMatch(property.Name, @"^Id$"))
                                    .ToArray();

            PropertyInfo propertyId = null;

            int propertyCount = properties.Count();

            for (int i = 0; i < propertyCount; i++)
            {
                propertyId = properties.ElementAt(i);
            }

            return propertyId;
        }

        /// <summary>
        ///     Atualiza uma ICollection de elementos e separa quais registros
        ///     da ICollection devem ser excluidos ou adicionados da tabela associativa
        ///     Para excluir o modelo que deve ser enviado para o backend deve ter no registro
        ///     as chaves estrangeiras zeradas.
        ///     Para incluir basta que o Id seja 0.
        /// </summary>
        /// <param name="entitiesToUpdate">ICollection</param>
        /// <returns>true</returns>
        public virtual bool UpdateMany(ICollection<TEntity> entitiesToUpdate)
        {
            if (entitiesToUpdate.Count == 0)
            {
                return true;
            }

            var type = entitiesToUpdate.First().GetType();

            var properties = type.GetProperties();

            PropertyInfo propertyId = null;

            PropertyInfo foreignId1 = null;

            PropertyInfo foreignId2 = null;

            int propertyCount = properties.Count();
            for (int i = 0; i < propertyCount; i++)
            {
                if (Regex.IsMatch(properties.ElementAt(i).Name, @"^Id$"))
                {
                    propertyId = properties.ElementAt(i);
                }
                else if (Regex.IsMatch(properties.ElementAt(i).Name, @"^Id[A-Z]"))
                {
                    if (foreignId1 == null)
                    {
                        foreignId1 = properties.ElementAt(i);
                    }
                    else
                    {
                        foreignId2 = properties.ElementAt(i);
                        break;
                    }
                }
            }

            var entitiesToInsert = new List<TEntity>();

            if (propertyId != null)
            {
                entitiesToInsert =
                    (from entity in entitiesToUpdate
                     let id = (int)propertyId.GetValue(entity, null)
                     where id == 0
                     select entity).ToList();
            }

            var entitiesToDelete = new List<TEntity>();

            if (foreignId1 != null && foreignId2 != null)
            {
                entitiesToDelete = (from entity in entitiesToUpdate
                                    let id1 = (int?)foreignId1.GetValue(entity, null)
                                    let id2 = (int?)foreignId2.GetValue(entity, null)
                                    where id1.HasValue && id1 == 0 && id2.HasValue && id2 == 0
                                    select entity).ToList();
            }
            var sucesso = InsertAll(entitiesToInsert.AsEnumerable());
            sucesso = sucesso && DeleteAll(entitiesToDelete);

            return sucesso;
        }

        public virtual T[] Select<T>(
            Expression<Func<TEntity, T>> keySelector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
        )
        {
            IQueryable<TEntity> query = this.set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            IQueryable<T> selectedQuery = query.Select(keySelector);

            return selectedQuery.ToArray();
        }

        public virtual async Task<T[]> SelectAsync<T>(
            Expression<Func<TEntity, T>> keySelector,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
        )
        {
            IQueryable<TEntity> query = this.set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            IQueryable<T> selectedQuery = query.Select(keySelector);

            return await selectedQuery.ToArrayAsync();
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return set.FirstOrDefault(filter);
        }

        public virtual TEntity[] Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            bool noTracking = false)
        {
            IQueryable<TEntity> query = this.set;

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToArray();
            }

            //deu um erro aqui não sei pq
            else
            {
                return query.ToArray();
            }
        }

        public virtual async Task<TEntity[]> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            bool noTracking = false)
        {
            IQueryable<TEntity> query = this.set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                if (!noTracking)
                {
                    return await orderBy(query).ToArrayAsync();
                }
                else
                {
                    return await orderBy(query).AsNoTracking().ToArrayAsync();
                }
            }
            else
            {
                if (!noTracking)
                {
                    return await query.ToArrayAsync();
                }
                else
                {
                    return await query.AsNoTracking().ToArrayAsync();
                }
            }
        }

        /// <summary>
        /// Attach an entity to context as it was recently taken form db
        /// now all changes made on this will be tracked
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual EntityEntry<TEntity> AttachEntityToContext(TEntity entity)
        {
            return set.Attach(entity);
        }

        /// <summary>
        /// Get the set from database for a more close linq operations on db
        ///  but now you will be susceptible to exceptions thrown by it
        /// </summary>
        /// <returns>Returns the Dbset</returns>
        public virtual DbSet<TEntity> Query()
        {
            return set;
        }

        public bool isEmpty()
        {
            return !set.Any();
        }

        public TEntity getFirst(Expression<Func<TEntity, bool>> expressao = null)
        {
            IQueryable<TEntity> query = set;

            if (expressao != null)
            {
                query = query.Where(expressao);
            }

            return query.FirstOrDefault();
        }

        public virtual bool SaveChanges()
        {
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return e == null;
            }
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return e == null;
            }
        }

        public TContext GetContext()
        {
            return context;
        }

        public void Dispose()
        {
            this.context.Dispose();
            GC.SuppressFinalize(this);
        }




        public virtual bool CanBeUpdate(TEntity entity)
        {
            var collections = entity.GetType().GetProperties().Where(x => x.PropertyType.IsAbstract).ToArray();

            int collectionsCount = collections.Count();

            for (int i = 0; i < collectionsCount; i++)
            {
                var collection = collections[i];
                var value = collection.GetValue(entity);
                var propertCount = value.GetType().GetProperties().FirstOrDefault();
                var associatedData = propertCount.GetValue(value);

                if ((int)associatedData > 0)
                {
                    return false;
                };
            }

            return true;
        }

    }

}
