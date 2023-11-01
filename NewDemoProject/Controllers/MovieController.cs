﻿using ApplicationLayer.Interface;
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
    public class MovieController : ControllerBase
    {
        public MovieController()
        {
          
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddNewMovie")]
        public async Task<IActionResult> AddNewMovie()
        {
            return Ok("Add New Movie");
        }

        [HttpPost]
        [Route("GetMovie")]
        public async Task<IActionResult> GetMovie()
        {
            return Ok("Get Movie");
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update()
        {
            return Ok("Get Movie");
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete()
        {
            return Ok("Get Movie");
        }
    }
}
