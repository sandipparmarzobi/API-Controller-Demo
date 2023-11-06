using API_Controller_Demo.Model;
using ApplicationLayer.Interface;
using ApplicationLayer.Services;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using InfrastructureLayer.Data;
using InfrastructureLayer.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewDemoProject.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using URF.Core.Abstractions;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookingService _bookingServie;
        private readonly IShowTimeService _showtimeServie;
        private readonly ITheaterService _theaterServie;
        private readonly IMovieService _movieServie;
        private readonly ISeatService _seatService;
        private readonly ISeatBookingService _seatBookingService;
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IMapper mapper, IBookingService bookingServie, IShowTimeService showtimeServie,
            ITheaterService theaterServie, IMovieService movieServie, ISeatService seatService, ISeatBookingService seatBookingService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _bookingServie = bookingServie;
            _showtimeServie = showtimeServie;
            _theaterServie = theaterServie;
            _movieServie = movieServie;
            _seatService = seatService;
            _seatBookingService = seatBookingService;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [Route("Get")]
        public Task<ActionResultData> Get()
        {
            var rtn = new ActionResultData();
            try
            {
                var booking = _bookingServie.FindAll();
                rtn.Data = booking;
                rtn.Status = Status.Success;
                return Task.FromResult(rtn);
            }
            catch (Exception ex)
            {
                rtn.Status = Status.Failed;
                rtn.Message = ex.Message;
                return Task.FromResult(rtn);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResultData> Add([FromBody] BookingDto booking)
        {
            var rtn = new ActionResultData();
            try
            {
                var userId = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                var username = HttpContext.User.Identity.Name;

                if (userId != booking.UserId)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "User is not authenticated";
                    return rtn;
                }

                var movie = _movieServie.FindAsync(booking.MovieId).Result;
                if (movie == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Movie is not found";
                    return rtn;
                }
                var showTime = _showtimeServie.FindAsync(booking.ShowTimeId).Result;
                if (showTime == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "ShowTime is not found";
                    return rtn;
                }

                //check seat is already reseved or not
                var userSeatNumbers = booking.SeatNumbers.Split(',');
                List<int> userSeatNumberIntList = new List<int>(Array.ConvertAll(userSeatNumbers, int.Parse));
                var reservedSeat = _seatService.FindReservedSeats(userSeatNumberIntList);
                if (reservedSeat != null && reservedSeat.Count > 0)
                {
                    var message = string.Join(",", reservedSeat);
                    rtn.Status = Status.Failed;
                    rtn.Message = "Seat Number " + message + " is already booked, try to select other seat number.";
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
                    // Calculate Movie Ticket Price
                    bookingEntity.TotalPrice = showTime.TicketPrice * bookingEntity.TotalTicket;
                    bookingEntity.UserId = Guid.Parse(userId);
                    bookingEntity.MovieId = movie.Id;
                    bookingEntity.ShowTimeId = showTime.Id;
                    _bookingServie.Insert(bookingEntity);

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
                    rtn.Status = Status.Success;
                    rtn.Message = "User Ticket Booking Added Successfully";
                    return rtn;
                }
                else
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Seats not found";
                    return rtn;
                }
            }
            catch (Exception ex)
            {
                rtn.Status = Status.Failed;
                rtn.Message += ex.Message;
                return rtn;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResultData> Update(Guid id, [FromBody] BookingDto updatedbooking)
        {
            var rtn = new ActionResultData();
            try
            {
                if (updatedbooking == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Invalid request data.";
                    return rtn;
                }
                var existingbooking = _bookingServie.FindAsync(id).Result;
                if (existingbooking == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Booking not found.";
                    return rtn;
                }
                existingbooking.UserId = Guid.Parse(updatedbooking.UserId);
                existingbooking.MovieId = Guid.Parse(updatedbooking.MovieId);
                existingbooking.ShowTimeId = Guid.Parse(updatedbooking.ShowTimeId);

                _bookingServie.Update(existingbooking);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Booking Updated Successfully";
                return rtn;
            }
            catch (Exception ex)
            {
                rtn.Status = Status.Failed;
                rtn.Message += ex.Message;
                return rtn;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResultData> Delete(Guid id)
        {
            var rtn = new ActionResultData();
            try
            {
                var existingBooking = _bookingServie.FindAsync(id).Result;
                if (existingBooking == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "ShowTime not found.";
                    return rtn;
                }
                _bookingServie.Delete(existingBooking);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Booking Deleted Successfully.";
                return rtn;
            }
            catch (Exception ex)
            {
                rtn.Status = Status.Failed;
                rtn.Message = ex.Message;
                return rtn;
            }
        }
    }
}
