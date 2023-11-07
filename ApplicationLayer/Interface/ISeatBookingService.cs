using DomainLayer.Entities;

namespace ApplicationLayer.Interface
{
    public interface ISeatBookingService : IServiceX<SeatBooking>
    {
        List<SeatBooking> FindSeatBookingByBookingId(Guid BookingId);
    }
}