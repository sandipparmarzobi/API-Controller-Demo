using ApplicationLayer.Interface;
using DomainLayer.Entities;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace ApplicationLayer.Services
{
    public class MovieService : Service<Movie>, IMovieService
    {
        public MovieService(ITrackableRepository<Movie> repository) : base(repository)
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
