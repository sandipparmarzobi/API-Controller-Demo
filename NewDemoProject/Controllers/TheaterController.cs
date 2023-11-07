using API_Controller_Demo.Model;
using ApplicationLayer.Interface;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using InfrastructureLayer.Data;
using InfrastructureLayer.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewDemoProject.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using URF.Core.Abstractions;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITheaterService _theaterServie;
        private readonly IUnitOfWork _unitOfWork;

        public TheaterController(IMapper mapper, ITheaterService theaterServie, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            this._theaterServie = theaterServie;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResultData> Get()
        {
            var rtn = new ActionResultData();
            try
            {
                var theaters = _theaterServie.FindAll();
                rtn.Data = theaters;
                rtn.Status = Status.Success;
                return rtn;
            }
            catch (Exception ex)
            {
                rtn.Status = Status.Failed;
                rtn.Message = ex.Message;
                return rtn;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResultData> Add([FromBody] TheaterDto theater)
        {
            var rtn = new ActionResultData();
            try
            {
                var theaterEntity = _mapper.Map<Theater>(theater);
                _theaterServie.Insert(theaterEntity);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Theater Added Successfully";
                return rtn;
            }
            catch (Exception ex)
            {
                rtn.Status = Status.Failed;
                rtn.Message += ex.Message;
                return rtn;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResultData> Update(Guid id, [FromBody] TheaterDto updatedTheater)
        {
            var rtn = new ActionResultData();
            try
            {
                if (updatedTheater == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Invalid request data.";
                    return rtn;
                }
                var existingTheater = _theaterServie.FindAsync(id).Result;
                if (existingTheater == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Theater not found.";
                    return rtn;
                }
                existingTheater.Name = updatedTheater.Name;
                existingTheater.Location = updatedTheater.Location;
                existingTheater.Capasity = updatedTheater.Capasity;

                _theaterServie.Update(existingTheater);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Theater Updated Successfully";
                return rtn;
            }
            catch (Exception ex)
            {
                rtn.Status = Status.Failed;
                rtn.Message += ex.Message;
                return rtn;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResultData> Delete(Guid id)
        {
            var rtn = new ActionResultData();
            try
            {
                var existingTheater = _theaterServie.FindAsync(id).Result;
                if (existingTheater == null)
                {
                    rtn.Status = Status.Failed;
                    rtn.Message = "Theater not found.";
                    return rtn;
                }
                _theaterServie.Delete(existingTheater);
                await _unitOfWork.SaveChangesAsync();
                rtn.Status = Status.Success;
                rtn.Message = "Theater Deleted Successfully.";
                return rtn;
            }
            catch (Exception ex)
            {
                rtn.Status = Status.Failed;
                rtn.Message = ex.Message;
                return rtn;
            }
        }
    }
}
