using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLayer.Services
{
    public class ReviewService : ServiceX<Reviews>, IReviewService
    {

        public ReviewService(IRepositoryX<Reviews> repository) : base(repository)
        {
        }
    }
}
