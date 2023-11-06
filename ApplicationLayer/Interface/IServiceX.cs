using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Services;

namespace ApplicationLayer.Interface
{
    public interface IServiceX<TEntity> : IService<TEntity>, IRepositoryX<TEntity> where TEntity : class, ITrackable
    {

    }
}
