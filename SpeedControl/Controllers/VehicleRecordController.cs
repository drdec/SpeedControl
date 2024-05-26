using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using SpeedControl.Domain.Interfaces;
using SpeedControl.Models;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace SpeedControl.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VehicleRecordController : ControllerBase
    {
        private readonly IVehicleRecordManager _vehicleRecordManager;

        public VehicleRecordController(IVehicleRecordManager vehicleRecordManager)
        {
            _vehicleRecordManager = vehicleRecordManager;
        }

        [HttpPost("addRecord")]
        public async Task<IActionResult> AddRecord(VehicleRecordRequest vehicleRecord)
        {
            try
            {
                await _vehicleRecordManager.AddRecordAsync(vehicleRecord);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("byDateAndSpeed")]
        public async Task<IActionResult> GetByDateAndSpeed(DateTime date,float speed)
        {
            var data = await _vehicleRecordManager.GetByDateAndSpeed(date, speed);

            if (data == null)
            {
                return Forbid("Запрос можно выполнять только в заданное время.");
            }

            return Ok(data);
        }

        [HttpGet("minAndMaxSpeed")]
        public async Task<IActionResult> GetMaxAndMinSpeed(DateTime date)
        {
            var data = await _vehicleRecordManager.GetMaxAndMinSpeedByDate(date);

            if (data.Item1 == -1 && data.Item2 == -1)
            {
                return Forbid("Запрос можно выполнять только в заданное время.");
            }

            //return Ok(data);
            return Ok(new { MaxSpeed = data.Item1, MinSpeed = data.Item2 });
        }

    }
}
