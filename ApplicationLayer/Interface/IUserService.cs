using ApplicationLayer.DTOs;
using DomainLayer.Entities;
using URF.Core.Abstractions.Services;

namespace ApplicationLayer.Interface
{
    public interface IUserService: IService<User>
    {
        //User? GetById(Guid id);
        //User? GetByName(string userName);

        List<AdminRegisterDto> GetAllUsers();
    }
}