using ApplicationLayer.Interface;
using DomainLayer.Entities;
using InfrastructureLayer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public SignInManager<ApplicationUser> _signInManager { get; set; }
        public UserManager<ApplicationUser> _userManager { get; set; }
        public RoleManager<ApplicationRole> _roleManager { get; }
        private readonly IConfiguration _configuration;
        private readonly MyDemoDBContext _context;

        public HomeController(IUserService userService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<ApplicationRole> roleManager, MyDemoDBContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("CreateRole")]
        //[Authorize(Policy = "AdminPolicy")]
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

        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        [Route("GetUserById")]
        //[Authorize(Policy = "UserPolicy, AdminPolicy")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("GetUserByName")]
        //[Authorize(Policy = "UserPolicy, AdminPolicy")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
