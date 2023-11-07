using ApplicationLayer.DTOs;
using ApplicationLayer.Interface;
using ApplicationLayer.Repository;
using AutoMapper;
using DomainLayer.Entities;
using URF.Core.Abstractions;

namespace ApplicationLayer.Services
{
    public class TheaterService : ServiceX<Theater>, ITheaterService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public TheaterService(IRepositoryX<Theater> repository, IMapper mapper, IUnitOfWork unitOfWork) : base(repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddTheater(TheaterDto theater)
        {
            var theaterEntity = _mapper.Map<Theater>(theater);
            Insert(theaterEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateTheater(Guid id, TheaterDto updatedTheater)
        {
            if (updatedTheater != null)
            {
                var existingTheater = await FindAsync(id) ?? throw new Exception("Theater not found.");
                existingTheater.Name = updatedTheater.Name;
                existingTheater.Location = updatedTheater.Location;
                existingTheater.Capasity = updatedTheater.Capasity;
                Update(existingTheater);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Invalid request data.");
            }
        }

        public async Task DeleteTheater(Guid id)
        {
            var existingTheater = await FindAsync(id) ?? throw new Exception("Theater not found.");
            await DeleteAsync(existingTheater);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
