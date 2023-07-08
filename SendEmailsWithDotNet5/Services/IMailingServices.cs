using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SendEmailsWithDotNet5.Services
{
    public interface IMailingServices
    {

        Task SendEmailAsync(string MailTo, string Subject, string body, IList<IFormFile> attachment = null);
    }
}
