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
    public class ShowTimeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IShowTimeService _showtimeServie;
        private readonly ITheaterService _theaterServie;
        private readonly IMovieService _movieServie;
        private readonly ISeatService _seatService;
        private readonly IUnitOfWork _unitOfWork;

        public ShowTimeController(IMapper mapper, IShowTimeService showtimeServie, 
            ITheaterService theaterServie, IMovieService movieServie, ISeatService seatService , IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _showtimeServie = showtimeServie;
            _theaterServie = theaterServie;
            _movieServie = movieServie;
            _seatService = seatService;
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
                var showtimes = _showtimeServie.FindAll();
                rtn.Data = showtimes;
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
        public async Task<ActionResultData> Add([FromBody] ShowTimeDto showtime)
        {
            var rtn = new ActionResultData();
            try
            {
                var theator = _theaterServie.FindAsync(showtime.TheaterId).Result;
                if (theator == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Theator is not found";
                    return rtn;
                }
                var movie = _movieServie.FindAsync(showtime.MovieId).Result;
                if (movie == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Movie is not found";
                    return rtn;
                }

                var showtimeEntity = _mapper.Map<ShowTime>(showtime);
                _showtimeServie.Insert(showtimeEntity);
                //await _unitOfWork.SaveChangesAsync();

                for (int i = 1; i <= theator.Capasity; i++)
                {
                    Seats seats = new()
                    {
                        Theater = theator,
                        TheaterId = theator.Id,
                        ShowTime = showtimeEntity,
                        ShowTimeId = showtimeEntity.Id,
                        SeatNumber = i
                    };
                    _seatService.Insert(seats);
                }
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Showtime Added Successfully";
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
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResultData> Update(Guid id, [FromBody] ShowTimeDto updatedshowtime)
        {
            var rtn = new ActionResultData();
            try
            {
                if (updatedshowtime == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Invalid request data.";
                    return rtn;
                }
                var existingshowtime = _showtimeServie.FindAsync(id).Result;
                if (existingshowtime == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Showtime not found.";
                    return rtn;
                }
                existingshowtime.StartTime = updatedshowtime.StartTime;
                existingshowtime.EndTime = updatedshowtime.EndTime;
                existingshowtime.MovieId = updatedshowtime.MovieId;
                existingshowtime.TheaterId = updatedshowtime.TheaterId;

                _showtimeServie.Update(existingshowtime);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Movie Updated Successfully";
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
                var existingMovie = _showtimeServie.FindAsync(id).Result;
                if (existingMovie == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "ShowTime not found.";
                    return rtn;
                }
                _showtimeServie.Delete(existingMovie);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "ShowTime Deleted Successfully.";
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
