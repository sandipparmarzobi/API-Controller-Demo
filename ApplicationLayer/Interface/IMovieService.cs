using ApplicationLayer.DTOs;
using DomainLayer.Entities;

namespace ApplicationLayer.Interface
{
    public interface IMovieService : IServiceX<Movie>
    {
        Task AddMovie(MovieDto movie);

        Task UpdateMovie(Guid id, MovieDto updatedMovie);

        Task DeleteMovie(Guid id);
    }
}