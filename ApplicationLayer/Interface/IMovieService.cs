using DomainLayer.Entities;

namespace ApplicationLayer.Interface
{
    public interface IMovieService : IServiceX<Movie>
    {
        Movie? GetById(Guid id);
        Movie? GetByName(string title);
    }
}