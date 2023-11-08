using ApplicationLayer.DTOs;
using ApplicationLayer.Interface;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieServie;

        public MovieController(IMovieService movieServie)
        {
            _movieServie = movieServie;
        }

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
        public async Task<ActionResultData> Add([FromForm] MovieDto movie)
        {
            var rtn = new ActionResultData();
            try
            {
                await _movieServie.AddMovie(movie);
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
                await _movieServie.UpdateMovie(id, updatedMovie);

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
                await _movieServie.DeleteMovie(id);
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
