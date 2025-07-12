using System.Threading.Tasks;
using MakesEasy.Interfaces;
using MakesEasy.Models;
using MakesEasy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleInterface _peopleRepo;
        private readonly JwtService _jwtService;

        public PeopleController(IPeopleInterface peopleRepo, JwtService jwtService)
        {
            _peopleRepo = peopleRepo;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("InsertPeople")]
        public async Task<IActionResult> InsertPeople([FromBody] PeopleModel people)
        {
            try
            {
                int vId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "villageId")?.Value);
                int tId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "talukaId")?.Value);
                int dId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "distId")?.Value);
                int sId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "stateId")?.Value);
                int cId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "countryId")?.Value);
                var waqts = new List<string> { "4 Month", "40 Days", "10 Days", "3 Days" };
                if (!waqts.Contains(people.Waqt))
                {
                    return Conflict(new { message = "Erorr" });
                }
                var peopels = await _peopleRepo.InsertPeople(people, vId, tId, dId, sId, cId);
                if (peopels == 1)
                {
                    return Ok(new { message = "People Inserted successfully", People = peopels });
                }
                else
                {
                    return BadRequest(new { message = "Error in Inserting People" });
                }

            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
                return BadRequest(new { message = "Error in Inserting" + ex.Message });
            }
        }
        [HttpGet]
        [Route("GetPeopleByVillage")]

        public async Task<IActionResult> GetPeopleByVillage()
        {
            try
            {
                string role = User.Claims.FirstOrDefault(c => c.Type == "type")?.Value;
                string claimId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                int villageId = Convert.ToInt32(claimId);

                var people = await _peopleRepo.GetPeopleByVillage(role, villageId);
                if (people != null)
                {
                    return Ok(new { message = "Getting Successfull", People = people });
                }
                else
                {
                    return BadRequest(new { message = "Error in GEtting" });
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
                return BadRequest(new { message = "Error in GEtting" + ex.Message });
            }
        }

        [HttpPut]
        [Route("UpdatePeople")]
        public async Task<IActionResult> UpdatePeople([FromBody] PeopleModel people)
        {
            try
            {
                var user = await _peopleRepo.UpdatePeople(people);
                if (user == 1)
                {
                    return Ok(new { message = "People updated successfully" });
                }
                else
                {
                    return BadRequest(new { message = "People Not Found" });
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
                return BadRequest(new { message = "People Not Found" + ex.Message });
            }
        }


        [HttpDelete]
        [Route("DeletePeople/{id}")]
        public async Task<IActionResult> DeletePeople(int id)
        {
            try
            {
                var user = await _peopleRepo.DeletePeople(id);
                if (user == 1)
                {
                    return Ok(new { message = "Deleted Successfully" });
                }
                else
                {
                    return BadRequest(new { message = "User not Found" });
                }

            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error: " + ex.Message);
                return BadRequest(new { message = "Error in Deleting" + ex.Message });
            }
        }

        [HttpGet]
        [Route("GetCount")]

        public async Task<IActionResult> GetCount()
        {
            try
            {
                string role = User.Claims.FirstOrDefault(c => c.Type == "type")?.Value;
                string claimId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                int villageId = Convert.ToInt32(claimId);
                var count = await _peopleRepo.GetCount(role, villageId);
                if (count != null)
                {
                    return Ok(new { message = "Count Getting Successfull", Count = count });
                }
                else
                {
                    return BadRequest(new { message = "Not Found" });
                }
            }
            catch (System.Exception ex)
            {

                System.Console.WriteLine("Error: " + ex.Message);
                return BadRequest(new { message = "Not Found" + ex.Message });
            }
        }

        [HttpPost("InsertStudent")]
        public async Task<IActionResult> InsertStudent(StudentModel student)
        {
            try
            {
                var villageId = User.FindFirst("villageId")?.Value;
                var talukaId = User.FindFirst("talukaId")?.Value;
                var distId = User.FindFirst("distId")?.Value;
                var stateId = User.FindFirst("stateId")?.Value;
                var countryId = User.FindFirst("countryId")?.Value;

                if (string.IsNullOrEmpty(villageId) || string.IsNullOrEmpty(talukaId) ||
                    string.IsNullOrEmpty(distId) || string.IsNullOrEmpty(stateId) ||
                    string.IsNullOrEmpty(countryId))
                {
                    return Unauthorized(new { message = "Invalid or expired token." });
                }

                student.VillageId = Convert.ToInt32(villageId);
                student.TalukaId = Convert.ToInt32(talukaId);
                student.DistId = Convert.ToInt32(distId);
                student.StateId = Convert.ToInt32(stateId);
                student.CountryId = Convert.ToInt32(countryId);

                var result = await _peopleRepo.InsertStudent(student);

                if (result == 1)
                {
                    return Ok(new { message = "Student Inserted Successfully", success = true, student.Name, student.Age });
                }
                else
                {
                    return BadRequest(new { message = "Error in Inserting Student" });
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = "Error Found: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("GetStudents")]
        public async Task<IActionResult> GetStudents()
        {

            var role = User.Claims.FirstOrDefault(c => c.Type == "type")?.Value;
            var claimId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            int id = Convert.ToInt32(claimId);
            var student = await _peopleRepo.GetStudents(id, role);
            return Ok(new { message = "Getting Students successfull", Students = student });
        }

        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(StudentModel student)
        {
            var update = await _peopleRepo.UpdateStudent(student);
            if (update == 1)
            {
                return Ok(new { message = "Student Updated Successfully", success = true });
            }
            else
            {
                return BadRequest(new { message = "Error in Updating student detail" });
            }
        }


        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var delete = await _peopleRepo.DeleteStudent(id);
            if (delete == 1)
            {
                return Ok(new { message = "Deleted Successfully" });
            }
            else
            {
                return BadRequest(new { message = "Error in Deleting" });
            }
        }
    }
}
