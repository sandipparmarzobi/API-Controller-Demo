using ApplicationLayer.DTOs;
using DomainLayer.Entities;

namespace ApplicationLayer.Interface
{
    public interface IBookingService : IServiceX<Booking>
    {
        public Task AddBooking(BookingDto booking);
        public Task UpdateBooking(Guid id, BookingDto booking);
        public Task DeleteBooking(Guid id);
    }
}