using ApplicationLayer.Interface;
using DomainLayer.Entities;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace ApplicationLayer.Services
{
    public class UserService : Service<User>, IUserService
    {
        public UserService(ITrackableRepository<User> repository) : base(repository)
        {
        }

        public User? GetById(Guid id)
        {
           return Repository.Queryable().FirstOrDefault(x => x.Id == id);
        }

        public User? GetByName(string userName)
        {
            return Repository.Queryable().FirstOrDefault(x => x.Username == userName);
        }
        public User? GetByNameQueryableSql(string userName)
        {
            return null;
        }
    }
}
