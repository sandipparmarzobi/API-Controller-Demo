using ApplicationLayer.DTOs;
using DomainLayer.Entities;

namespace ApplicationLayer.Interface
{
    public interface IReviewService : IServiceX<Reviews>
    {
        Task AddReview(ReviewDto review);
        Task UpdateReview(Guid id, ReviewDto updatedReview);
        Task DeleteReview(Guid id);
    }
}