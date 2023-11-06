using ApplicationLayer.Interface;
using TrackableEntities.Common.Core;
using URF.Core.Services;

namespace ApplicationLayer.Repository
{
    public class ServiceX<TEntity> : Service<TEntity>, IServiceX<TEntity> where TEntity : class, ITrackable
    {
        private readonly IRepositoryX<TEntity> repository;

        protected ServiceX(IRepositoryX<TEntity> repository) : base(repository)
        {
            this.repository = repository;
        }

        public IEnumerable<TEntity> FindAll()
        {
           return repository.FindAll();
        }
    }
}
