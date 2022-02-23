using Catalog.API.Context;
using Catalog.API.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalog.API.Respository
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancelToken = default);
        IRepositoryBase<TSet> GetRepository<TSet>(Type? clazz = default) where TSet : class;
        Task CommitAsync(CancellationToken cancelToken = default);
        Task RollBackAsync(CancellationToken cancelToken = default);

    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatalogDBContext _repositoryContext;
        private readonly IDbContextTransaction _transaction;
        private readonly Dictionary<Type, object> _repositories;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UnitOfWork(CatalogDBContext repositoryContext, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryContext = repositoryContext;
            _transaction = _repositoryContext.Database.BeginTransaction();
            _repositories = new Dictionary<Type, object>();
            _httpContextAccessor = httpContextAccessor;
        }

        //private string CurrentUserId => _httpContextAccessor.HttpContext.User.Identity.Name;
        public async Task CommitAsync(CancellationToken cancelToken = default)
        {
            try
            {
                await _transaction.CommitAsync(cancelToken);
            }
            catch
            {
                await _transaction.RollbackAsync(cancelToken);
            }
        }

        public IRepositoryBase<TSet> GetRepository<TSet>(Type? clazz = default) where TSet : class
        {
            if (_repositories.ContainsKey(typeof(TSet)))
            {
                var repositorySet = _repositories[typeof(TSet)] as IRepositoryBase<TSet>;

                if (repositorySet == null)
                    throw new NullReferenceException();

                return repositorySet;
            }

            var repository = clazz == default ? new RepositoryBase<TSet>(_repositoryContext) : Activator.CreateInstance(clazz, _repositoryContext) as IRepositoryBase<TSet>;

            if (repository == null)
                throw new NullReferenceException();

            _repositories.Add(typeof(TSet), repository);
            return repository;
        }

        public Task RollBackAsync(CancellationToken cancelToken = default)
        {
            return _transaction.RollbackAsync(cancelToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancelToken = default)
        {

            var entries = _repositoryContext.ChangeTracker.Entries<BaseEntity>().ToArray();
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedOn = DateTime.UtcNow;
                    entity.UpdatedBy = string.Empty;//CurrentUserId;
                }
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                    entity.CreatedBy = string.Empty;//CurrentUserId;
                }
            }
            return _repositoryContext.SaveChangesAsync(cancelToken);
        }
    }
}
