using DomainLayer.Entities;
using URF.Core.Abstractions.Services;

namespace ApplicationLayer.Interface
{
    public interface IMovieService : IService<Movie>
    {
        Movie? GetById(Guid id);
        Movie? GetByName(string title);
    }
}