using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLayer.Services
{
    public class SeatService : ServiceX<Seats>, ISeatService
    {

        public SeatService(IRepositoryX<Seats> repository) : base(repository)
        {
        }

        public Seats? FindBySeatNumber(int SeatNumber)
        {
            return Repository.Queryable().FirstOrDefault(x=>x.SeatNumber == SeatNumber);
        }

        public List<int> FindReservedSeats(List<int> seatNumbers)
        {
            var reservedSeatNumbers = Repository.Queryable()
                .Where(seat => seatNumbers.Contains(seat.SeatNumber) && seat.IsReserved)
                .Select(seat => seat.SeatNumber)
                .ToList();

            return reservedSeatNumbers;
        }

        public List<Seats> FindSeatsbySeatNumbers(List<int> seatNumbers)
        {
            var reservedSeatNumbers = Repository.Queryable()
                .Where(seat => seatNumbers.Contains(seat.SeatNumber))
                .ToList();

            return reservedSeatNumbers;
        }
    }
}
