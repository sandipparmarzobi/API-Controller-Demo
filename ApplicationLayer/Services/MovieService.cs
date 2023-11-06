using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using DomainLayer.Entities;

namespace ApplicationLayer.Services
{
    public class MovieService : ServiceX<Movie>, IMovieService
    {

        public MovieService(IRepositoryX<Movie> repository) : base(repository)
        {
        }

        public Movie? GetById(Guid id)
        {
           return Repository.Queryable().FirstOrDefault(x => x.Id == id);
        }

        public Movie? GetByName(string title)
        {
            return Repository.Queryable().FirstOrDefault(x => x.Title == title);
        }
    }
}
