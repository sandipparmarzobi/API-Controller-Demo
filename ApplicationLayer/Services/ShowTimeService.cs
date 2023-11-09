using ApplicationLayer.DTOs;
using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using InfrastructureLayer.Data;
using Microsoft.EntityFrameworkCore;
using URF.Core.Abstractions;

namespace ApplicationLayer.Services
{
    public class ShowTimeService : ServiceX<ShowTime>, IShowTimeService
    {
        private readonly MyDemoDBContext _context;
        private readonly IMapper _mapper;
        private readonly ITheaterService _theaterServie;
        private readonly IMovieService _movieServie;
        private readonly ISeatService _seatService;
        private readonly IUnitOfWork _unitOfWork;

        public ShowTimeService(IRepositoryX<ShowTime> repository, MyDemoDBContext context, IMapper mapper,
            ITheaterService theaterServie, IMovieService movieServie, ISeatService seatService, IUnitOfWork unitOfWork) : base(repository)
        {
            _context = context;
            _mapper = mapper;
            _theaterServie = theaterServie;
            _movieServie = movieServie;
            _seatService = seatService;
            _unitOfWork = unitOfWork;
        }

        public async Task AddShowTime(ShowTimeDto showtime)
        {
            var theator = _theaterServie.FindById(showtime.TheaterId) ?? throw new Exception("Theator is not found");
            var movie = _movieServie.FindById(showtime.MovieId) ?? throw new Exception("Movie is not found");

            var showtimeEntity = _mapper.Map<ShowTime>(showtime);
            Insert(showtimeEntity);
            for (int i = 1; i <= theator.Capacity; i++)
            {
                Seats seats = new()
                {
                    TheaterId = theator.Id,
                    ShowTimeId = showtimeEntity.Id,
                    SeatNumber = i
                };
                _seatService.Insert(seats);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateShowTime(Guid id, ShowTimeDto updatedshowtime)
        {
            if (updatedshowtime != null)
            {
                var existingshowtime = FindById(id) ?? throw new Exception("Showtime not found.");
                var theator = _theaterServie.FindById(updatedshowtime.TheaterId) ?? throw new Exception("Theator is not found");
                var movie =  _movieServie.FindById(updatedshowtime.MovieId) ?? throw new Exception("Movie is not found");

                var existingSeats = _seatService.FindSeatsbyTheatorAndShowTime(theator.Id,existingshowtime.Id);
                foreach (var item in existingSeats)
                {
                    _seatService.Delete(item);
                }

                existingshowtime.StartTime = updatedshowtime.StartTime;
                existingshowtime.EndTime = updatedshowtime.EndTime;
                existingshowtime.MovieId = updatedshowtime.MovieId;
                existingshowtime.TheaterId = updatedshowtime.TheaterId;
                existingshowtime.Screen = updatedshowtime.Screen;
                existingshowtime.TicketPrice = updatedshowtime.TicketPrice;
                Update(existingshowtime);

                for (int i = 1; i <= theator.Capacity; i++)
                {
                    Seats seats = new()
                    {
                        TheaterId = theator.Id,
                        ShowTimeId = existingshowtime.Id,
                        SeatNumber = i
                    };
                    _seatService.Insert(seats);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Invalid request data.");
            }
        }

        public async Task DeleteShowTime(Guid id)
        {
            var existingShowtime = FindById(id) ?? throw new Exception("ShowTime not found.");
            Delete(existingShowtime);
            await _unitOfWork.SaveChangesAsync();
        }

        public ShowTime? GetShowTimeIncludeSeats(Guid id)
        {
            return _context.ShowTime.Where(x => x.Id == id).Include(x => x.Seats).FirstOrDefault();
        }
    }
}
