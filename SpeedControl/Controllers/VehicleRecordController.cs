using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace SpeedControl.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VehicleRecordController : ControllerBase
    {
        [HttpPost("addRecord")]
        public async Task<IActionResult> AddRecord()
        {
            return Ok();
        }

        [HttpGet("byDateAndSpeed")]
        public async Task<IActionResult> GetByDateAndSpeed()
        {
            return Ok();
        }

        [HttpGet("minAndMaxSpeed")]
        public async Task<IActionResult> GetMaxAndMinSpeed()
        {
            return Ok();
        }

    }
}
