using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Trackable;

namespace ApplicationLayer.Interface
{
    public interface IRepositoryX<TEntity> : ITrackableRepository<TEntity> where TEntity : class, ITrackable
    {
        public IEnumerable<TEntity> FindAll();
        public TEntity? FindById(Guid id);
    }
}
