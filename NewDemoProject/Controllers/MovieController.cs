using API_Controller_Demo.Model;
using ApplicationLayer.Interface;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewDemoProject.Model;
using URF.Core.Abstractions;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMovieService _movieServie;
        private readonly IUnitOfWork _unitOfWork;

        public MovieController(IMapper mapper, IMovieService movieServie, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            this._movieServie = movieServie;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResultData> Get()
        {
            var rtn = new ActionResultData();
            try
            {
                var movies =  _movieServie.FindAll();
                rtn.Data = movies;
                rtn.Status = Status.Success;
                return rtn;
            }
            catch (Exception ex)
            {
                rtn.Status = Status.Failed;
                rtn.Message=ex.Message;
                return rtn;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResultData> Add([FromBody] MovieDto movie)
        {
            var rtn = new ActionResultData();
            try
            {
                var movieEntity = _mapper.Map<Movie>(movie);
                _movieServie.Insert(movieEntity);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Movie Added Successfully";
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
        public async Task<ActionResultData> Update(Guid id, [FromBody] MovieDto updatedMovie)
        {
            var rtn = new ActionResultData();
            try
            {
                if (updatedMovie == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Invalid request data.";
                    return rtn;
                }
                var existingMovie = _movieServie.GetById(id);
                if (existingMovie == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Movie not found.";
                    return rtn;
                }
                existingMovie.Title = updatedMovie.Title;
                existingMovie.ReleaseDate = updatedMovie.ReleaseDate;
                existingMovie.Genre = Enum.Parse<MovieGenre>(updatedMovie.Genre);
                existingMovie.Description = updatedMovie.Description;
                existingMovie.Duration = updatedMovie.Duration;
                existingMovie.Director = updatedMovie.Director;
                existingMovie.PosterURL = updatedMovie.PosterURL;
                existingMovie.TrailerURL = updatedMovie.TrailerURL;
                _movieServie.Update(existingMovie);
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
                var existingMovie = _movieServie.GetById(id);
                if (existingMovie == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Movie not found.";
                    return rtn;
                }
                _movieServie.Delete(existingMovie);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Movie Deleted Successfully.";
                return rtn;
            }
            catch (Exception ex)
            {
                rtn.Status  = Status.Failed;
                rtn.Message=ex.Message;
                return rtn;
            }
        }
    }
}
