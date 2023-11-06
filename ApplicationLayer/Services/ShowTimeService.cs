using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using DomainLayer.Entities;

namespace ApplicationLayer.Services
{
    public class ShowTimeService : ServiceX<ShowTime>, IShowTimeService
    {
        public ShowTimeService(IRepositoryX<ShowTime> repository) : base(repository)
        {
        }
    }
}
