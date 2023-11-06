using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLayer.Services
{
    public class SeatBookingService : ServiceX<SeatBooking>, ISeatBookingService
    {

        public SeatBookingService(IRepositoryX<SeatBooking> repository) : base(repository)
        {
        }
    }
}
