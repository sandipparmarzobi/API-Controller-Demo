using ApplicationLayer.Interface;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ApplicationLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration) {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }    

        public async Task<ApplicationUser> AuthenticateAsync(string username, string password)
        {
           return await _userManager.FindByEmailAsync(username);
        }
    }
}
