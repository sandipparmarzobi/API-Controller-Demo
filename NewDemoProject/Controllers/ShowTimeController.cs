using ApplicationLayer.DTOs;
using ApplicationLayer.Interface;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimeController : ControllerBase
    {
        private readonly IShowTimeService _showtimeService;
        public ShowTimeController(IShowTimeService showtimeService)
        {
            _showtimeService = showtimeService;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [Route("Get")]
        public Task<ActionResultData> Get()
        {
            var rtn = new ActionResultData();
            try
            {
                var showtimes = _showtimeService.FindAll();
                rtn.Data = showtimes;
                rtn.Status = Status.Success;
                return Task.FromResult(rtn);
            }
            catch (Exception ex)
            {
                rtn.Status = Status.Failed;
                rtn.Message = ex.Message;
                return Task.FromResult(rtn);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResultData> Add([FromBody] ShowTimeDto showtime)
        {
            var rtn = new ActionResultData();
            try
            {
                await _showtimeService.AddShowTime(showtime);
                rtn.Status = Status.Success;
                rtn.Message = "Showtime Added Successfully";
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
        public async Task<ActionResultData> Update(Guid id, [FromBody] ShowTimeDto updatedshowtime)
        {
            var rtn = new ActionResultData();
            try
            {
                await _showtimeService.UpdateShowTime(id,updatedshowtime);
                rtn.Status = Status.Success;
                rtn.Message = "Showtime Updated Successfully";
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
                await _showtimeService.DeleteShowTime(id);
                rtn.Status = Status.Success;
                rtn.Message = "ShowTime Deleted Successfully.";
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
