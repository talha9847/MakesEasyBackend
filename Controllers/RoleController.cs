using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public string Role;

        [HttpPost("SetRoles/{id}/{role}")]
        public IActionResult SetRoles(int id, string role)
        {
            try
            {
                HttpContext.Session.SetString("role", role);
                HttpContext.Session.SetInt32("id", id);

                Role = HttpContext.Session.GetString("role");
                System.Console.WriteLine(Role);

                return Ok(new { message = "Role Setting Successfull" });

            }
            catch (System.Exception ex)
            {

                System.Console.WriteLine("Error :" + ex.Message);
                return BadRequest(new { message = "Error in Setting Role: " + ex.Message });
            }
        }

        [HttpGet("Validate")]

        public IActionResult Validate()
        {
            try
            {
                var role = HttpContext.Session.GetString("role");
                var villageId = HttpContext.Session.GetInt32("villageId");

                if (role == null || villageId == 0)
                {
                    return Unauthorized(new { message = "You have not logged in" });
                }
                return Ok(new { message = "You are Authorized" });
            }
            catch (System.Exception ex)
            {

                System.Console.WriteLine("Error :" + ex.Message);
                return BadRequest(new { message = "Error in :" + ex.Message });
            }
        }
    }
}
