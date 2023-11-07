using DomainLayer.Entities;

namespace ApplicationLayer.Interface
{
    public interface ISeatService : IServiceX<Seats>
    {
        public Seats? FindBySeatNumber(int SeatNumber);
        public List<int> FindReservedSeats(List<int> seatNumbers);
        public List<Seats> FindSeatsbySeatNumbers(List<int> seatNumbers);
        public List<Seats> FindSeatsbyTheatorAndShowTime(Guid theatorId,Guid showtimeId);
    }
}