using ApplicationLayer.Interface;
using DomainLayer.Entities;
using InfrastructureLayer.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewDemoProject.Model;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Transactions;
using URF.Core.Abstractions;

namespace NewDemoProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        public SignInManager<ApplicationUser> _signInManager { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }
        public RoleManager<ApplicationRole> _roleManager { get; }

        private readonly IConfiguration _configuration;

        private readonly MyDemoDBContext _context;

        public LoginController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration,
            RoleManager<ApplicationRole> roleManager,
            MyDemoDBContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User not found");
                }
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var appUser = new ApplicationUser
                    {
                        UserName = user.Username,
                        FirstName = user.Username,
                        SecondName = user.Username,
                        Email = user.Email,
                    };

                    var userResult = await _userManager.CreateAsync(appUser, user.Password);
                    if (userResult.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(user.Role))
                        {
                            transaction.Rollback();
                            return BadRequest("Role does not exist.");
                        }

                        await _userManager.AddToRoleAsync(appUser, user.Role);
                        transaction.Commit();

                        return Ok("User created and assigned to role.");
                    }
                    else
                    {
                        // Handle user creation errors, for example, duplicate username or password requirements not met.
                        return BadRequest("User creation failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userrole = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "admin")
                };

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return BadRequest("Login failed");
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            // Optionally, sign the user out of any external authentication providers like Google or Facebook.
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return Ok(new { message = "Logout successful" });
        }
    }
}