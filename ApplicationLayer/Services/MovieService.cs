using ApplicationLayer.DTOs;
using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using URF.Core.Abstractions;

namespace ApplicationLayer.Services
{
    public class MovieService : ServiceX<Movie>, IMovieService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public MovieService(IRepositoryX<Movie> repository, IMapper mapper, IUnitOfWork unitOfWork) : base(repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddMovie(MovieDto movie)
        {
            var movieEntity = _mapper.Map<Movie>(movie);
            Insert(movieEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMovie(Guid id)
        {
            var existingMovie = GetById(id) ?? throw new Exception("Movie not found.");
            Delete(existingMovie);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateMovie(Guid id, MovieDto updatedMovie)
        {
            if (updatedMovie == null)
            {
                throw new Exception("Invalid request data.");
            }
            var existingMovie = GetById(id) ?? throw new Exception("Movie not found.");
            existingMovie.Title = updatedMovie.Title;
            existingMovie.ReleaseDate = updatedMovie.ReleaseDate;
            existingMovie.Genre = Enum.Parse<MovieGenre>(updatedMovie.Genre);
            existingMovie.Description = updatedMovie.Description;
            existingMovie.Duration = updatedMovie.Duration;
            existingMovie.Director = updatedMovie.Director;
            existingMovie.PosterURL = updatedMovie.PosterURL;
            existingMovie.TrailerURL = updatedMovie.TrailerURL;
            Update(existingMovie);
            await _unitOfWork.SaveChangesAsync();
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
