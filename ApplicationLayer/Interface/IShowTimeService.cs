using ApplicationLayer.DTOs;
using DomainLayer.Entities;

namespace ApplicationLayer.Interface
{
    public interface IShowTimeService : IServiceX<ShowTime>
    {
        public ShowTime? GetShowTimeIncludeSeats(Guid id);
        public Task AddShowTime(ShowTimeDto showTime);
        public Task UpdateShowTime(Guid id, ShowTimeDto showTime);
        public Task DeleteShowTime(Guid id);
        public Task<ShowTimeDataDto> GetShowTimeData();

        public Task<List<ShowTimeDto>> GetShowTimeDataIncludMoiveAndTheater();
        public Task<ShowTimeDto> GetById(Guid Id);
    }
}