using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using DomainLayer.Entities;

namespace ApplicationLayer.Services
{
    public class SeatBookingService : ServiceX<SeatBooking>, ISeatBookingService
    {

        public SeatBookingService(IRepositoryX<SeatBooking> repository) : base(repository)
        {
        }

        public List<SeatBooking> FindSeatBookingByBookingId(Guid BookingId)
        {
            return Repository.Queryable().Where(x => x.BookingId == BookingId).ToList();
        }
    }
}
