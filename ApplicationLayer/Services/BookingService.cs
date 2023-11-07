using ApplicationLayer.DTOs;
using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using AutoMapper;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using URF.Core.Abstractions;

namespace ApplicationLayer.Services
{
    public class BookingService : ServiceX<Booking>, IBookingService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IShowTimeService _showtimeServie;
        private readonly IMovieService _movieServie;
        private readonly ISeatService _seatService;
        private readonly ISeatBookingService _seatBookingService;
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IRepositoryX<Booking> repository, IHttpContextAccessor httpContextAccessor, 
            IMapper mapper, IShowTimeService showtimeServie, IMovieService movieServie,
            ISeatService seatService, ISeatBookingService seatBookingService, IUnitOfWork unitOfWork) : base(repository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _showtimeServie = showtimeServie;
            _movieServie = movieServie;
            _seatService = seatService;
            _seatBookingService = seatBookingService;
            _unitOfWork = unitOfWork;
        }

        public async Task AddBooking(BookingDto booking)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var loggedInUserId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID in the token.");
            }

            var movie = await _movieServie.FindAsync(booking.MovieId) ?? throw new Exception("Movie is not found");
            var showTime = await _showtimeServie.FindAsync(booking.ShowTimeId) ?? throw new Exception("ShowTime is not found");

            var userSeatNumbers = booking.SeatNumbers.Split(',');
            List<int> userSeatNumberIntList = new List<int>(Array.ConvertAll(userSeatNumbers, int.Parse));
            var reservedSeat = _seatService.FindReservedSeats(userSeatNumberIntList);
            if (reservedSeat != null && reservedSeat.Count > 0)
            {
                var message = string.Join(",", reservedSeat);
                throw new Exception("Seat Number " + message + " is already reserved, try to select other seat number.");
            }

            var seats = _seatService.FindSeatsbySeatNumbers(userSeatNumberIntList);
            if (seats != null && seats.Count > 0)
            {
                foreach (var item in seats)
                {
                    item.IsReserved = true;
                    _seatService.Update(item);
                }

                var bookingEntity = _mapper.Map<Booking>(booking);
                bookingEntity.TotalTicket = userSeatNumbers.Length;
                bookingEntity.TotalPrice = showTime.TicketPrice * bookingEntity.TotalTicket;
                bookingEntity.UserId = booking.UserId;
                bookingEntity.MovieId = movie.Id;
                bookingEntity.ShowTimeId = showTime.Id;
                Insert(bookingEntity);

                foreach (var item in seats)
                {
                    SeatBooking seatBooking = new()
                    {
                        SeatNumber = item.SeatNumber,
                        BookingId = bookingEntity.Id,
                        SeatId = item.Id
                    };
                    _seatBookingService.Insert(seatBooking);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Seats not found");
            }
        }

        public async Task UpdateBooking(Guid id, BookingDto booking)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var loggedInUserId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID in the token.");
            }

            var movie = await _movieServie.FindAsync(booking.MovieId) ?? throw new Exception("Movie is not found");
            var showTime = await _showtimeServie.FindAsync(booking.ShowTimeId) ?? throw new Exception("ShowTime is not found");

            var userSeatNumbers = booking.SeatNumbers.Split(',');
            List<int> userSeatNumberIntList = new List<int>(Array.ConvertAll(userSeatNumbers, int.Parse));
            var reservedSeat = _seatService.FindReservedSeats(userSeatNumberIntList);
            if (reservedSeat != null && reservedSeat.Count > 0)
            {
                var message = string.Join(",", reservedSeat);
                throw new Exception("Seat Number " + message + " is already reserved, try to select other seat number.");
            }

            var seats = _seatService.FindSeatsbySeatNumbers(userSeatNumberIntList);
            if (seats != null && seats.Count > 0)
            {
                foreach (var item in seats)
                {
                    item.IsReserved = true;
                    _seatService.Update(item);
                }

                var bookingEntity = _mapper.Map<Booking>(booking);
                bookingEntity.TotalTicket = userSeatNumbers.Length;
                bookingEntity.TotalPrice = showTime.TicketPrice * bookingEntity.TotalTicket;
                bookingEntity.UserId = booking.UserId;
                bookingEntity.MovieId = movie.Id;
                bookingEntity.ShowTimeId = showTime.Id;
                Insert(bookingEntity);

                foreach (var item in seats)
                {
                    SeatBooking seatBooking = new()
                    {
                        SeatNumber = item.SeatNumber,
                        BookingId = bookingEntity.Id,
                        SeatId = item.Id
                    };
                    _seatBookingService.Insert(seatBooking);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Seats not found");
            }
        }

        public async Task DeleteBooking(Guid id)
        {
            var existingBooking = FindById(id) ?? throw new Exception("Booking is not found");
            var seatBooking = _seatBookingService.FindSeatBookingByBookingId(existingBooking.Id);
            if (seatBooking != null && seatBooking.Count > 0)
            {
                foreach (var item in seatBooking)
                {
                    {
                        item.Seats.IsReserved = false;
                        _seatService.Update(item.Seats);
                        _seatBookingService.Delete(item);
                    }
                }
                Delete(existingBooking);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task CancelBooking(Guid id)
        {
            var existingBooking = FindById(id) ?? throw new Exception("Booking is not found");
            var seatBooking = _seatBookingService.FindSeatBookingByBookingId(existingBooking.Id);
            if (seatBooking != null && seatBooking.Count > 0)
            {
                foreach (var item in seatBooking)
                {
                    {
                         item.Seats.IsReserved = false;
                        _seatService.Update(item.Seats);
                    }
                }
                existingBooking.BookingStatus = DomainLayer.Enums.BookingStatus.Cancelled;
                Update(existingBooking);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Seat Booking Not found");
            }
        }
    }
}
