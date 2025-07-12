using System.Threading.Tasks;
using MakesEasy.Interfaces;
using MakesEasy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class FourMonthController : ControllerBase
    {
        private readonly IFourMonthInterface _fourMonthReop;
        public FourMonthController(IFourMonthInterface fourMonthRepo)
        {
            _fourMonthReop = fourMonthRepo;
        }

        [HttpGet("GetFourMonth")]
        public async Task<IActionResult> GetCompanions()
        {
            int roleId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var role = Convert.ToString(User.Claims.FirstOrDefault(c => c.Type == "type")?.Value);
            var result = await _fourMonthReop.Get4Companions(role, roleId);
            return Ok(new
            {
                message = "Found Successfully",
                result

            });
        }

        [HttpGet("GetCompanion/{id}")]
        public async Task<IActionResult> GetCompanionData(int id)
        {
            var result = await _fourMonthReop.GetData(id);
            if (result != null)
            {
                return Ok(new { message = "Get Companion Data Successfulll", result });
            }
            else
            {
                return BadRequest(new { message = "Not.." });
            }
        }


        [HttpGet("GetFourtyDays")]
        public async Task<IActionResult> GetFourtyDays()
        {
            int roleId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var role = Convert.ToString(User.Claims.FirstOrDefault(c => c.Type == "type")?.Value);
            var result = await _fourMonthReop.Get40Companion(role, roleId);

            if (result != null)
            {
                return Ok(new { message = "Getting Successfull", result });
            }
            else
            {
                return BadRequest(new { message = "Error Found Successfully" });
            }
        }
        [HttpGet("Get40Data/{id}")]
        public async Task<IActionResult> Get40Companion(int id)
        {
            var result = await _fourMonthReop.Get40Data(id);
            if (result != null)
            {
                return Ok(new { message = "Getting Successfull", result });
            }
            else
            {
                return BadRequest(new { message = "Error Found Successfully" });
            }
        }
        [HttpPost("Add4Companion")]
        public async Task<IActionResult> Add4Companion([FromForm] FourMonthModel model)
        {
            var result = await _fourMonthReop.AddCompanionsData(model);
            if (result == 1)
            {
                return Ok(new { message = "Successfully", model });
            }
            else
            {
                return BadRequest(new { message = "Error in Adding" });
            }
        }

        [HttpPut("UpdateCompanions")]
        public async Task<IActionResult> UpdateCompanionsData([FromForm] FourMonthModel model)
        {
            var result = await _fourMonthReop.UpdateCompanionsData(model);
            if (result == 1)
            {
                return Ok(new { message = "Edited Successfullyy" });
            }
            else
            {
                return BadRequest(new { message = "Error in Editing" });
            }
        }

        [HttpPost("Add40Companion")]
        public async Task<IActionResult> AddCompanion([FromForm] FourMonthModel model)
        {
            if (String.IsNullOrEmpty(model.Place))
            {
                return Conflict(new { message = "Required" });
            }
            var result = await _fourMonthReop.AddCompanionData(model);
            if (result == 1)
            {
                return Ok(new { message = "Added Successfully" });
            }
            else
            {
                return BadRequest(new { message = "Error found successfully" });
            }
        }
        [HttpPut("UpdateCompanion")]
        public async Task<IActionResult> UpdateCompanion([FromForm]FourMonthModel model)
        {
            var result = await _fourMonthReop.UpdateCompanionData(model);
            if (result == 1)
            {
                return Ok(new { message = "Updated successfully" });
            }
            else
            {
                return BadRequest(new { message = "Error Found" });
            }
        }
    }
}
