using ApplicationLayer.Interface;
using ApplicationLayer.Services;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URF.Core.Abstractions;

namespace NewDemoProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        public SignInManager<ApplicationUser> _signInManager { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public LoginController(IUnitOfWork unitOfWork, IUserService userService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
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
                    Password = user.Password,
                    PasswordHash = user.Password,
                    FirstName = user.Username,
                    SecondName = user.Username,
                    Email = user.Email
                };

               await _userManager.CreateAsync(appUser,user.Password);
               //await _unitOfWork.SaveChangesAsync();
             }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("GetUserById")]
        public IActionResult GetUserById(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        [Route("GetUserByName")]
        public IActionResult GetUserByName(string name)
        {
            var user = _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}