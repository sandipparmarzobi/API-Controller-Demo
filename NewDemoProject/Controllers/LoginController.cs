using ApplicationLayer.Interface;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewDemoProject.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using URF.Core.Abstractions;

namespace NewDemoProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LoginController : ControllerBase
    {
        public SignInManager<ApplicationUser> _signInManager { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }
        public RoleManager<ApplicationRole> _roleManager { get; }

        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public LoginController(IUserService userService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<ApplicationRole> roleManager)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User not found");
                }
                var appUser = new ApplicationUser
                {
                    UserName = user.Username,
                    FirstName = user.Username,
                    SecondName = user.Username,
                    Email = user.Email,
                };
                
                var userResult = await _userManager.CreateAsync(appUser, user.Password);
                var roleResult = await _userManager.AddToRoleAsync(appUser, user.Role);
                if (userResult.Succeeded && roleResult.Succeeded)
                {
                    return Ok("Registration successful");
                }
                return BadRequest(userResult.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
   
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);

               var userrole= await _userManager.GetRolesAsync(user);

                //var addRoleResult = await _userManager.AddToRoleAsync(user, "Admin");
                //if (addRoleResult.Succeeded)
                //{
                //    return Ok("Role assigned successfully");
                //}
                //else
                //{
                //    // Handle the case where role assignment fails
                //    return BadRequest(addRoleResult.Errors);
                //}

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return BadRequest("Login failed");
        }

        [HttpGet]
        [Route("GetUserById")]
        [Authorize(Roles = "Admin")]
     
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("GetUserByName")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var newRole = new ApplicationRole { Name = roleName };
                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    return Ok("Role created successfully.");
                }

                return BadRequest("Role creation failed.");
            }
            return BadRequest("Role already exists.");
        }
    }
}