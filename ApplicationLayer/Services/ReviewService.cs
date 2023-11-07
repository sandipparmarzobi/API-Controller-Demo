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
    public class ReviewService : ServiceX<Reviews>, IReviewService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMovieService _movieServie;
        private readonly IUnitOfWork _unitOfWork;
        public ReviewService(IRepositoryX<Reviews> repository, IHttpContextAccessor httpContextAccessor, IMapper mapper,  IMovieService movieServie, IUnitOfWork unitOfWork) : base(repository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _movieServie = movieServie;
            _unitOfWork = unitOfWork;
        }

        public async Task AddReview(ReviewDto review)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out _))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID in the token.");
            }
            _ = await _movieServie.FindAsync(review.MovieId) ?? throw new Exception("Movie is not found");
            var reviewEntity = _mapper.Map<Reviews>(review);
            Insert(reviewEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateReview(Guid id, ReviewDto updatedReview)
        {
            if (updatedReview != null)
            {
                var existingReview = FindById(id) ?? throw new Exception("Review not found.");
                existingReview.UserId = updatedReview.UserId;
                existingReview.MovieId = updatedReview.MovieId;
                existingReview.Ratting = updatedReview.Ratting;
                existingReview.Comments = updatedReview.Comments;
                existingReview.ReviewDate = DateTime.Now;
                Update(existingReview);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Invalid request data.");
            }
        }

        public async Task DeleteReview(Guid id)
        {
            var existingReview = FindById(id) ?? throw new Exception("Review not found.");
            Delete(existingReview);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
