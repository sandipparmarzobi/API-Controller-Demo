using ApplicationLayer.Interface;
using ApplicationLayer.Services;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using URF.Core.Abstractions;

namespace NewDemoProject.Model
{
    public class UserRegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Email { get; set; }
    }
}