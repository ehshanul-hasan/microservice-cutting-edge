using Catalog.API.Model.Entity;
using Catalog.API.Respository;
using Common.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services
{
    public interface IPackageService
    {
        Task<int> CreateAsync(Package client, CancellationToken cancelToken = default);
        Task<Package> GetByIdAsync(int id, CancellationToken cancelToken = default);
        Task<bool> UpdateAsync(Package request, CancellationToken cancelToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancelToken = default);
        Task<List<Package>> ListAsync(CancellationToken cancelToken = default);
    }

    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepositoryBase<Package> _packageRepository;
        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _packageRepository = _unitOfWork.GetRepository<Package>();
        }
        public async Task<int> CreateAsync(Package package, CancellationToken cancelToken = default)
        {
            await _packageRepository.Create(package);

            await _unitOfWork.SaveChangesAsync(cancelToken);

            return package.Id;
        }

        public async Task<Package> GetByIdAsync(int id, CancellationToken cancelToken = default)
        {
            Package package = await _packageRepository.Get(id, cancelToken);

            return package;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancelToken = default)
        {
            Package package = await _packageRepository.Get(id);

            if (package == null)
                throw new NotFoundException("Offering Type Not Found");

            _packageRepository.Delete(package);

            var result = await _unitOfWork.SaveChangesAsync(cancelToken);

            return result > 0;
        }

        public async Task<bool> UpdateAsync(Package request, CancellationToken cancelToken = default)
        {
            Package package = await _packageRepository.Get(request.Id);

            if (package == null)
                throw new NotFoundException("client Not Found");

            package.Title = request.Title;
            package.Description = request.Description;
            package.Cost = request.Cost;
            package. IsActive = request.IsActive;

            _packageRepository.Update(package);

            var result = await _unitOfWork.SaveChangesAsync(cancelToken);

            return result > 0;
        }

        public async Task<List<Package>> ListAsync(CancellationToken cancelToken = default)
        {
            var result = await _packageRepository.Where(s => s.Id > 0).ToListAsync(cancelToken);

            return result;
        }
    }
}
