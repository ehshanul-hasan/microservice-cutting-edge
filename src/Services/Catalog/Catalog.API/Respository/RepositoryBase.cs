using Catalog.API.Context;
using Common.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalog.API.Respository
{
    public interface IRepositoryBase<T>
    {
        Task<T> Get(int id, CancellationToken cancelToken = default);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate, CancellationToken cancelToken = default);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Where(Expression<Func<T, bool>> predicatebool, bool readOnly = false);
    }
    public class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : class
    {
        private CatalogDBContext _repositoryContext { get; set; }

        public RepositoryBase(CatalogDBContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<T> Get(int id, CancellationToken cancelToken = default)
        {
            var entity = await _repositoryContext.Set<T>().FindAsync(id);

            if (entity == null)
                throw new NotFoundException("Entity Not Found");

            return entity;
        }
        public async Task Create(T entity)
        {
            await _repositoryContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _repositoryContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _repositoryContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool readOnly = false)
        {
            var set = _repositoryContext.Set<T>();
            if (readOnly)
                set.AsNoTracking();

            return set.Where(predicate);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate, CancellationToken cancelToken = default)
        {
            var entity = await _repositoryContext.Set<T>().FirstOrDefaultAsync(predicate, cancelToken);

            return entity;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _repositoryContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
