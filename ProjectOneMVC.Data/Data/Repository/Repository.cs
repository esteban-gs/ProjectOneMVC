using Microsoft.EntityFrameworkCore;
using ProjectOneMVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectOneMVC.Data.Data.Repository
{
    public abstract class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : ApplicationDbContext
    {
        private readonly TContext context;
        public Repository(TContext context)
        {
            this.context = context;
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Get(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }
        virtual public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            var entitiesQ = context.Set<TEntity>().Where(expression);
            return entitiesQ;
        }

        public virtual IQueryable<TEntity> GetAllEagerly(Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            var result = context.Set<TEntity>();

            IQueryable<TEntity> resultWithEagerLoading = func(result);

            return resultWithEagerLoading;
        }
    }
}
