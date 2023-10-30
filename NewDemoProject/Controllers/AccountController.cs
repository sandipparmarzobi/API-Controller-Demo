using ApplicationLayer.Interface;
using DomainLayer.Entities;
using InfrastructureLayer.Data;
using InfrastructureLayer.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewDemoProject.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly SignInManager<ApplicationUser> _signInManager;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly MyDemoDBContext _context;
        private readonly IEmailService _emailService;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, JwtTokenHelper jwtTokenHelper, MyDemoDBContext context, IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenHelper = jwtTokenHelper;
            _context = context;
            _emailService = emailService;
        }

        [Authorize(Roles = "Admin")]
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
                else
                {
                    return BadRequest("Role creation failed.");
                }
            }
            return BadRequest("Role already exists.");
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
                // Model validations
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // To check duplicate email or not
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "The email address is already in use. Please choose a different email address.");
                    return BadRequest(new { errors = ModelState });
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                var appUser = new ApplicationUser
                {
                    UserName = user.Username,
                    FirstName = user.Username,

                    Email = user.Email,
                };
                var userResult = await _userManager.CreateAsync(appUser, user.Password);
                if (userResult.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(user.Role))
                    {
                        transaction.Rollback();
                        return BadRequest("Provided Role does not exist.");
                    }
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    var tokenLink = Url.Action("ConfirmEmail", "User", new { userId = appUser.Id, token }, Request.Scheme);
                    if (!_emailService.SendEmail("sandip.parmar@zobiwebsolutions.com", string.Empty, "Email Confirmation", tokenLink))
                    {
                        transaction.Rollback();
                        return BadRequest("User not created due to email service issue.");
                    }
                    await _userManager.AddToRoleAsync(appUser, user.Role);
                    transaction.Commit();
                    return Ok("User created and send the email for account confirmation");
                }
                else
                {
                    return BadRequest(userResult.Errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
