using DomainLayer.Entities;
using InfrastructureLayer.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewDemoProject.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public SignInManager<ApplicationUser> _signInManager { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }
        public RoleManager<ApplicationRole> _roleManager { get; }

        private readonly JwtTokenHelper _jwtTokenHelper;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, JwtTokenHelper jwtTokenHelper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenHelper = jwtTokenHelper;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // User and Mail confirmation checking
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, "You must confirm your email before logging in.");
                return BadRequest(ModelState);
            }
            // Password checking
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                int? expirationMinutes = null;

                // Create claims for the login user for the generate token.
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Iat,new DateTimeOffset(DateTime.Now).ToString())
                };
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var claimsIdentity = new ClaimsIdentity(claims, "JWT");

                // For Manual Handle Remember Me
                if (model.RememberMe)
                {
                    var expiresInMinutes = model.RememberMe ? 7 * 24 * 60 : 15;
                    expirationMinutes = expiresInMinutes;
                }
                // Generate a JWT token using the JwtTokenHelper.
                string token = _jwtTokenHelper.GenerateToken(claimsIdentity, expirationMinutes);

                return Ok(new
                {
                    Token = token,
                });
            }
            return Unauthorized();
        }  
    }
}
