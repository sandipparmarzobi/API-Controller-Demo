using ApplicationLayer.DTOs;
using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.EntityFrameworkCore;
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
            if (movie.ImageFile != null)
            {
                // Save the image in the database
                using (var memoryStream = new MemoryStream())
                {
                    await movie.ImageFile.CopyToAsync(memoryStream);
                    movieEntity.Image = memoryStream.ToArray();
                }
            }
            else
            {
                throw new Exception("Please Upload Movie Image");
            }
            Insert(movieEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMovie(Guid id)
        {
            var existingMovie = FindById(id) ?? throw new Exception("Movie not found.");
            Delete(existingMovie);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateMovie(Guid id, MovieDto updatedMovie)
        {
            if (updatedMovie == null)
            {
                throw new Exception("Invalid request data.");
            }
            var existingMovie = FindById(id) ?? throw new Exception("Movie not found.");
            if (updatedMovie.ImageFile != null)
            {
                // Save the image in the database
                using (var memoryStream = new MemoryStream())
                {
                    await updatedMovie.ImageFile.CopyToAsync(memoryStream);
                    existingMovie.Image = memoryStream.ToArray();
                }
            }
            existingMovie.Title = updatedMovie.Title;
            existingMovie.ReleaseDate = updatedMovie.ReleaseDate;
            existingMovie.Genre = Enum.Parse<MovieGenre>(updatedMovie.Genre);
            existingMovie.Description = updatedMovie.Description;
            existingMovie.Duration = updatedMovie.Duration;
            existingMovie.Director = updatedMovie.Director;
            existingMovie.TrailerURL = updatedMovie.TrailerURL;
            Update(existingMovie);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
