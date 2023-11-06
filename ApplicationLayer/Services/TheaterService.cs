using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using DomainLayer.Entities;

namespace ApplicationLayer.Services
{
    public class TheaterService : ServiceX<Theater>, ITheaterService
    {

        public TheaterService(IRepositoryX<Theater> repository) : base(repository)
        {
        }

    }
}
