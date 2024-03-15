using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        protected readonly DbSet<TEntity> _entities;
        protected readonly TradeMarketDbContext _context;
#pragma warning restore CA1051 // Do not declare visible instance fields
        protected Repository(TradeMarketDbContext context)
        {
            _entities = context?.Set<TEntity>() ?? throw new System.ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Repository<TEntity> repository &&
                   EqualityComparer<DbSet<TEntity>>.Default.Equals(_entities, repository._entities);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_entities);
        }

        public virtual void Update(TEntity entity)
        {
            _entities.Update(entity);
        }
    }
}
