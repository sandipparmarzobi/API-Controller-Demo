using ApplicationLayer.DTOs;
using ApplicationLayer.Interface;
using AutoMapper;
using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URF.Core.Abstractions;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterController : ControllerBase
    {
        private readonly ITheaterService _theaterServie;

        public TheaterController(ITheaterService theaterServie)
        {
           _theaterServie = theaterServie;
        }

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
                await _theaterServie.AddTheater(theater);
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
                await _theaterServie.UpdateTheater(id, updatedTheater);
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
                await _theaterServie.DeleteTheater(id);
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
