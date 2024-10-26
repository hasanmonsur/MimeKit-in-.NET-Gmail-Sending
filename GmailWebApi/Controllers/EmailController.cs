using GmailWebApi.Helpers;
using GmailWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GmailWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (string.IsNullOrEmpty(emailRequest.To) || string.IsNullOrEmpty(emailRequest.Subject) || string.IsNullOrEmpty(emailRequest.Body))
            {
                return BadRequest("Email fields are required.");
            }

            await GmailServiceHelper.SendEmailAsync(emailRequest.To, emailRequest.Subject, emailRequest.Body);
            return Ok("Email sent successfully.");
        }
    }
}
