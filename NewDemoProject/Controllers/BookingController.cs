using ApplicationLayer.DTOs;
using ApplicationLayer.Interface;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Controller_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingServie;
        public BookingController(IBookingService bookingServie)
        {
            _bookingServie = bookingServie;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [Route("Get")]
        public Task<ActionResultData> Get()
        {
            var rtn = new ActionResultData();
            try
            {
                var booking = _bookingServie.FindAll();
                rtn.Data = booking;
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
        public async Task<ActionResultData> Add([FromBody] BookingDto booking)
        {
            var rtn = new ActionResultData();
            try
            {
               await _bookingServie.AddBooking(booking);
               rtn.Status = Status.Success;
               rtn.Message = "Booking Added Successfully";
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
        public async Task<ActionResultData> Update(Guid id, [FromBody] BookingDto updatedbooking)
        {
            var rtn = new ActionResultData();
            try
            {
                await _bookingServie.UpdateBooking(id, updatedbooking);
                rtn.Status = Status.Success;
                rtn.Message = "Booking Updated Successfully";
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
                await _bookingServie.DeleteBooking(id);
                rtn.Status = Status.Success;
                rtn.Message = "Booking Deleted Successfully.";
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
        [HttpDelete]
        [Route("Cancel")]
        public async Task<ActionResultData> CancelBooking(Guid id)
        {
            var rtn = new ActionResultData();
            try
            {
                await _bookingServie.CancelBooking(id);
                rtn.Status = Status.Success;
                rtn.Message = "Booking Cancelled Successfully.";
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
