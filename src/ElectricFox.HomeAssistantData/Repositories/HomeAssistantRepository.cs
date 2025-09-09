using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElectricFox.HomeAssistantData.Repositories
{
    public class HomeAssistantRepository
    {
        private readonly HomeAssistantContext _dbContext;

        private readonly SpecificationEvaluator _specificationEvaluator;

        public HomeAssistantRepository(HomeAssistantContext context)
        {
            _dbContext = context;
            _specificationEvaluator = SpecificationEvaluator.Default;
        }

        public T? Find<T>(int  id)
            where T : class
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T Add<T>(T entity)
            where T: class
        {
            _dbContext.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T?> FindAsync<T>(int id, CancellationToken cancellationToken = default)
            where T : class
        {
            return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
        }

        public async Task<List<T>> ListAsync<T>(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
            where T : class
        {
            return await ApplySpecification(specification).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<T?> FirstOrDefaultAsync<T>(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
            where T : class
        {
            return await ApplySpecification(specification)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private IQueryable<T> ApplySpecification<T>(
            ISpecification<T> specification,
            bool evaluateCriteriaOnly = false)
            where T : class
        {
            return _specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
