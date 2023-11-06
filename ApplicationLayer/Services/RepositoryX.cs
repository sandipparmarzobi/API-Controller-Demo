using ApplicationLayer.Interface;
using Microsoft.EntityFrameworkCore;
using TrackableEntities.Common.Core;
using URF.Core.EF.Trackable;

namespace ApplicationLayer.Repository
{
    public class RepositoryX<TEntity> : TrackableRepository<TEntity>, IRepositoryX<TEntity> where TEntity : class, ITrackable
    {
        public RepositoryX(DbContext context) : base(context)
        {

        }

        // Example: adding synchronous Find, scope: application-wide
        public TEntity Find(object[] keyValues, CancellationToken cancellationToken = default)
        {
            return this.Context.Find<TEntity>(keyValues) as TEntity;
        }

        // Method to find all records
        public IEnumerable<TEntity> FindAll()
        {
            return this.Context.Set<TEntity>();
        }
    }
}
