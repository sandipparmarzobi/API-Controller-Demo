using ApplicationLayer.Interface;
using Microsoft.EntityFrameworkCore;
using TrackableEntities.Common.Core;
using URF.Core.EF.Trackable;

namespace ApplicationLayer.Repository
{
    public class RepositoryX<TEntity> : TrackableRepository<TEntity>, IRepositoryX<TEntity> where TEntity : class, ITrackable
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public RepositoryX(DbContext context) : base(context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<TEntity>();
        }

        // Method to find all records
        public IEnumerable<TEntity> FindAll()
        {
            return this.Context.Set<TEntity>();
        }

        public TEntity? FindById(Guid id)
        {
            return _dbSet.Find(id);
        }
    }
}
