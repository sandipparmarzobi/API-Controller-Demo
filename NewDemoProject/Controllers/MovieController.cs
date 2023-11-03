using ApplicationLayer.Interface;
using AutoMapper;
using DomainLayer.Entities;
using InfrastructureLayer.Data;
using InfrastructureLayer.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewDemoProject.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok("Get Movie");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddNewMovie")]
        public async Task<IActionResult> Add([FromBody] MovieModel movie)
        {
            var movieEntity = _mapper.Map<Movie>(movie);
            _movieServie.Insert(movieEntity);
            await _unitOfWork.SaveChangesAsync();
            return Ok("Movie Added Successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MovieModel updatedMovie)
        {
            if (updatedMovie == null)
            {
                return BadRequest("Invalid request data.");
            }

            var existingMovie = _movieServie.GetById(id);
            if (existingMovie == null)
            {
                return NotFound(); // Resource not found
            }
            var New= _mapper.Map(existingMovie,updatedMovie);
            var movieEntity = _mapper.Map<Movie>(New);
            _movieServie.Update(movieEntity);
            await _unitOfWork.SaveChangesAsync();
            return Ok("Movie Updated Successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movieItem = _movieServie.GetById(id);
            if (movieItem != null)
            {
                _movieServie.Delete(movieItem);
                await _unitOfWork.SaveChangesAsync();
            }
            return Ok("Movie Deleted Successfully");
        }
    }
}
