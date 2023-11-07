using ApplicationLayer.DTOs;
using DomainLayer.Entities;

namespace ApplicationLayer.Interface
{
    public interface ITheaterService : IServiceX<Theater>
    {
        Task AddTheater(TheaterDto theater);
        Task UpdateTheater(Guid id, TheaterDto updatedTheater);
        Task DeleteTheater(Guid id);
    }
}