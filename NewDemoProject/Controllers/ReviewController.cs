using ApplicationLayer.DTOs;
using ApplicationLayer.Interface;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using URF.Core.Abstractions;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IReviewService _reviewService;
        private readonly IMovieService _movieServie;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewController(IMapper mapper, IReviewService reviewService, IMovieService movieServie, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _reviewService = reviewService;
            _movieServie = movieServie;
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
                var reviews = _reviewService.FindAll();
                rtn.Data = reviews;
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
        public async Task<ActionResultData> Add([FromBody] ReviewDto review)
        {
            var rtn = new ActionResultData();
            try
            {
                var userId = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                var username = HttpContext.User.Identity.Name;

                if (userId != review.UserId)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "User is not authenticated";
                    return rtn;
                }

                var movie = _movieServie.FindAsync(review.MovieId).Result;
                if (movie == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Movie is not found";
                    return rtn;
                }

                var reviewEntity = _mapper.Map<Reviews>(review);
                _reviewService.Insert(reviewEntity);

                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Review Added Successfully";
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
        public async Task<ActionResultData> Update(Guid id, [FromBody] ReviewDto updatedReview)
        {
            var rtn = new ActionResultData();
            try
            {
                if (updatedReview == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Invalid request data.";
                    return rtn;
                }
                var existingReview = _reviewService.FindAsync(id).Result;
                if (existingReview == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Review not found.";
                    return rtn;
                }

                existingReview.UserId = Guid.Parse(updatedReview.UserId);
                existingReview.MovieId = Guid.Parse(updatedReview.MovieId);
                existingReview.Ratting = updatedReview.Ratting;
                existingReview.Comments = updatedReview.Comments;
                existingReview.ReviewDate = DateTime.Now;

                _reviewService.Update(existingReview);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Review Updated Successfully";
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
                var existingReview = _reviewService.FindAsync(id).Result;
                if (existingReview == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Review not found.";
                    return rtn;
                }
                _reviewService.Delete(existingReview);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Review Deleted Successfully.";
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
