using Microsoft.AspNetCore.Mvc;
using SendEmailsWithDotNet5.Dtos;
using SendEmailsWithDotNet5.Services;
using System.Threading.Tasks;

namespace SendEmailsWithDotNet5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailingController : ControllerBase
    {
        private readonly MailingServices _mailingServices;

        public MailingController(MailingServices mailingServices)
        {
            _mailingServices = mailingServices;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendMail(MailRequestDto dto)
        {
            await _mailingServices.SendEmailAsync(dto.ToEmail, dto.Subjects, dto.Body, dto.Attachments);
            return Ok();
        }
    }
}
