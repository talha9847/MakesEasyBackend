using System.Threading.Tasks;
using MakesEasy.Interfaces;
using MakesEasy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageInterface _msgRepo;
        public MessageController(IMessageInterface msgRepo)
        {
            _msgRepo = msgRepo;
        }

        [HttpPost("PostMessage")]
        public async Task<IActionResult> PostMessage(MessageModel msg)
        {
            var result = await _msgRepo.PostMessage(msg);
            if (result == 1)
            {
                return Ok(new { message = "Message Post Successfulllyy" });
            }
            else
            {
                return BadRequest(new { message = "Error in sending Message" });
            }
        }
    }
}
