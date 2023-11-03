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
    public class TheaterController : ControllerBase
    {
        public TheaterController()
        {
          
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            return Ok("Get Movie");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddNewMovie")]
        public async Task<IActionResult> Add()
        {
            return Ok("Add New Movie");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update()
        {
            return Ok("Get Movie");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete()
        {
            return Ok("Get Movie");
        }
    }
}
