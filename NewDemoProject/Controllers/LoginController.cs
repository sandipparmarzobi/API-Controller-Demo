using ApplicationLayer.Interface;
using ApplicationLayer.Services;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using URF.Core.Abstractions;

namespace NewDemoProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public LoginController(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        [HttpPost]
        [Route("InsertUser")]
        public async Task<IActionResult> InsertUser([FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User not found");
                }
               _userService.Insert(user);
               await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(Guid id)
        {
            var user = _userService.GetById(id);
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
            var user = _userService.GetByName(name);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}