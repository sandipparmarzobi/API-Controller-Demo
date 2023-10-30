using ApplicationLayer.Interface;
using DomainLayer.Entities;

namespace ApplicationLayer.Services
{
    public class AuthService : IAuthService
    {
        public Task<ApplicationUser> AuthenticateAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
