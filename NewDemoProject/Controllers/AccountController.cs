using API_Controller_Demo.Model;
using ApplicationLayer.Interface;
using DomainLayer.Entities;
using InfrastructureLayer.Data;
using InfrastructureLayer.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResultData> Login([FromBody] LoginDto model)
        {
            var rtn = new ActionResultData();
            try
            {
                // User and Mail confirmation checking
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
                {
                    rtn.Message = "You must confirm your email before logging in.";
                    return rtn;
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
                    // Generate a JWT token using the JwtTokenHelper class.
                    string token = _jwtTokenHelper.GenerateToken(claimsIdentity, expirationMinutes);

                    rtn.Message = "\"User created and send the email for account confirmation";
                    rtn.Status = DomainLayer.Enums.Status.Success;
                    rtn.Data = token;
                    return rtn;
                }
                else
                {
                    rtn.Message = "Invalid Email or Password";
                    return rtn;
                }
            }
            catch (Exception ex)
            {
                rtn.Message = ex.Message;
                return rtn;
            }
        }

        [HttpPost]
        [Route("RegisterNormalUser")]
        public async Task<ActionResultData> RegisterNormalUser([FromBody] UserRegisterDto user)
        {
            var rtn = new ActionResultData();
            try
            {
                if (user == null)
                {
                    rtn.Message = "User not found";
                    return rtn;
                }
                if (!ModelState.IsValid)
                {
                    string? errorList = GetModelErrors();
                    rtn.Message = errorList;
                    return rtn;
                }
                // To check duplicate email or not
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    rtn.Message = "The email address is already in use. Please choose a different email address.";
                    return rtn;
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                ApplicationUser appUser = ConvertToApplicationUser(user);
                var userResult = await _userManager.CreateAsync(appUser, user.Password);
                if (userResult.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("User"))
                    {
                        transaction.Rollback();
                        rtn.Message = "Provided Role does not exist.";
                        return rtn;
                    }
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    var tokenLink = Url.Action("ConfirmEmail", "User", new { userId = appUser.Id, token }, Request.Scheme);
                    if (!_emailService.SendEmail("sandip.parmar@zobiwebsolutions.com", string.Empty, "Email Confirmation", tokenLink))
                    {
                        transaction.Rollback();
                        rtn.Message = "User not created due to email service issue.";
                        return rtn;
                    }
                    await _userManager.AddToRoleAsync(appUser, "User");
                    transaction.Commit();
                    rtn.Message = "User created and send the email for account confirmation";
                    rtn.Status = DomainLayer.Enums.Status.Success;
                    return rtn;
                }
                else
                {
                    var message = string.Join(", ", userResult.Errors.Select(x => x.Description));
                    rtn.Message = message;
                    return rtn;
                }
            }
            catch (Exception ex)
            {
                rtn.Message = ex.Message;
                return rtn;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("RegisterAdminUser")]
        public async Task<ActionResultData> RegisterAdminUser([FromBody] AdminRegisterDto user)
        {
            var rtn = new ActionResultData();
            try
            {
                if (user == null)
                {
                    rtn.Message = "User not found";
                    return rtn;
                }
                if (!ModelState.IsValid)
                {
                    string? errorList = GetModelErrors();
                    rtn.Message = errorList;
                    return rtn;
                }
                // To check duplicate email or not
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    rtn.Message = "The email address is already in use. Please choose a different email address.";
                    return rtn;
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = user.Username,
                    FullName = user.FullName,
                    RegistrationDate = DateTime.UtcNow,
                    Email = user.Email,
                };
                var userResult = await _userManager.CreateAsync(appUser, user.Password);
                if (userResult.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(user.Role))
                    {
                        transaction.Rollback();
                        rtn.Message = "Provided Role does not exist.";
                        return rtn;
                    }
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    var tokenLink = Url.Action("ConfirmEmail", "User", new { userId = appUser.Id, token }, Request.Scheme);
                    if (!_emailService.SendEmail("sandip.parmar@zobiwebsolutions.com", string.Empty, "Email Confirmation", tokenLink))
                    {
                        transaction.Rollback();
                        rtn.Message = "User not created due to email service issue.";
                        return rtn;
                    }
                    await _userManager.AddToRoleAsync(appUser, user.Role);
                    transaction.Commit();
                    rtn.Message = "User created and send the email for account confirmation";
                    rtn.Status = DomainLayer.Enums.Status.Success;
                    return rtn;
                }
                else
                {
                    var message = string.Join(", ", userResult.Errors.Select(x => x.Description));
                    rtn.Message = message;
                    return rtn;
                }
            }
            catch (Exception ex)
            {
                rtn.Message = ex.Message;
                return rtn;
            }
        }

        private static ApplicationUser ConvertToApplicationUser(UserRegisterDto user)
        {
            return new ApplicationUser
            {
                UserName = user.Username,
                FullName = user.FullName,
                RegistrationDate = DateTime.UtcNow,
                PhoneNumber = user.Phone,
                Email = user.Email,
            };
        }

        private string? GetModelErrors()
        {
            return (from item in ModelState.Values
                    from error in item.Errors
                    select error.ErrorMessage).ToString();
        }
    }
}
