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
            var theator = _theaterServie.FindById(Guid.Parse(showtime.Theater)) ?? throw new Exception("Theator is not found");
            var movie = _movieServie.FindById(Guid.Parse(showtime.Movie)) ?? throw new Exception("Movie is not found");

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
                var theator = _theaterServie.FindById(Guid.Parse(updatedshowtime.Theater)) ?? throw new Exception("Theator is not found");
                var movie =  _movieServie.FindById(Guid.Parse(updatedshowtime.Movie)) ?? throw new Exception("Movie is not found");

                var existingSeats = _seatService.FindSeatsbyTheatorAndShowTime(theator.Id,existingshowtime.Id);
                foreach (var item in existingSeats)
                {
                    _seatService.Delete(item);
                }

                existingshowtime.StartTime = Convert.ToDateTime(updatedshowtime.StartTime);
                existingshowtime.EndTime = Convert.ToDateTime(updatedshowtime.EndTime);
                existingshowtime.MovieId = Guid.Parse (updatedshowtime.Movie);
                existingshowtime.TheaterId = Guid.Parse(updatedshowtime.Theater);
                existingshowtime.Screen = Convert.ToChar(updatedshowtime.Screen);
                existingshowtime.TicketPrice = Convert.ToDecimal(updatedshowtime.TicketPrice);
                existingshowtime.HideShowTime = updatedshowtime.HideShowTime.Value;
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
        public Task<ShowTimeDataDto> GetShowTimeData()
        {
            ShowTimeDataDto showTimeDataDto = new ShowTimeDataDto();
            List<MovieDataDto> movieList = new List<MovieDataDto>();
            List<TheaterDataDto> theaterList = new List<TheaterDataDto>();
            var movies = _movieServie.FindAll();
            foreach (var movie in movies)
            {
                movie.ImageBase64 = Convert.ToBase64String(movie.Image);
                var moviedto = _mapper.Map<MovieDataDto>(movie);
                movieList.Add(moviedto);
            }
            showTimeDataDto.MovieList = movieList;

            var theaters = _theaterServie.FindAll();
            foreach (var theater in theaters)
            {
                var theaterdto = _mapper.Map<TheaterDataDto>(theater);
                theaterdto.Name += " - "+theater.Location;
                theaterList.Add(theaterdto);
            }
            showTimeDataDto.TheaterList = theaterList;
            return Task.FromResult(showTimeDataDto);
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

        public async Task<List<ShowTimeDto>> GetShowTimeDataIncludMoiveAndTheater()
        {
            List<ShowTimeDto> showTimeList = new List<ShowTimeDto>();
            var showTime = _context.ShowTime.Include(x => x.Movie).Include(x => x.Theater).ToList();
            foreach (var item in showTime)
            {
                var showtimeDto = _mapper.Map<ShowTimeDto>(item);
                showtimeDto.Id = item.Id.ToString();
                showtimeDto.Movie = item.Movie.Title; 
                showtimeDto.StartTime = item.StartTime.ToString("t");
                showtimeDto.EndTime = item.EndTime.ToString("t");
                showtimeDto.Theater = item.Theater.Name + " - " + item.Theater.Location;
                showtimeDto.HideShowTime = item.HideShowTime;
                showTimeList.Add(showtimeDto);
            }
            return showTimeList;
        }
        public async Task<ShowTimeDto> GetById(Guid Id)
        {
            var showTime = _context.ShowTime.Where(x=>x.Id== Id).Include(x => x.Movie).Include(x => x.Theater).FirstOrDefault();
            if (showTime != null)
            {
                var showtimeDto = _mapper.Map<ShowTimeDto>(showTime);
                return showtimeDto;
            }
            return null;
        }

    }
}
