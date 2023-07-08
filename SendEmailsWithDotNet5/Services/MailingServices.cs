using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using SendEmailsWithDotNet5.Setting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SendEmailsWithDotNet5.Services
{
    public class MailingServices : IMailingServices
    {
        private readonly MailSetting _mailSetting;

        public MailingServices(IOptions<MailSetting> mailSetting) =>
       
            _mailSetting = mailSetting.Value;
        
        public async Task SendEmailAsync(string MailTo, string Subject, string body, IList<IFormFile> attachment = null)
        {
            var email = new MimeMessage
            {
                    Sender = MailboxAddress.Parse(_mailSetting.Email),
                    Subject = Subject,
            };

            email.To.Add(MailboxAddress.Parse(MailTo)); 
            
            var builder = new BodyBuilder();
            if (attachment != null)
            {
                byte[] fileBytes;
                foreach (var File in attachment)
                {
                    if (File.Length > 0)
                    {
                        using var MS = new MemoryStream();
                        File.CopyTo(MS);
                        fileBytes = MS.ToArray();
                        builder.Attachments.Add(File.FileName , fileBytes , ContentType.Parse(File.ContentType));
                    }
                }
            }
            builder.HtmlBody= body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.Email));

            using var Smtp = new SmtpClient();
            Smtp.Connect(_mailSetting.Host, _mailSetting.Port , SecureSocketOptions.SslOnConnect);
            Smtp.Authenticate(_mailSetting.Email,_mailSetting.Password);
            await Smtp.SendAsync(email);
            Smtp.Disconnect(true);
        }
    }
}
