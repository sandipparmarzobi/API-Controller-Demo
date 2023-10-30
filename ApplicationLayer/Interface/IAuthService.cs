using DomainLayer.Entities;
using URF.Core.Abstractions.Services;

namespace ApplicationLayer.Interface
{
    public interface IAuthService
    {
        Task<ApplicationUser> AuthenticateAsync(string username, string password);
    }
}