using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using DomainLayer.Entities;

namespace ApplicationLayer.Services
{
    public class BookingService : ServiceX<Booking>, IBookingService
    {

        public BookingService(IRepositoryX<Booking> repository) : base(repository)
        {
        }
    }
}
