using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Auction_System_Library_Database.Models;


namespace Auction_System_Library_Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        public EmailService(IOptions<EmailSettings> options)

        {

            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));

        }
        private SmtpClient CreateSmtpClient()

        {

            return new SmtpClient(_settings.SmtpHost)

            {

                Port = _settings.SmtpPort,

                EnableSsl = true,

                Credentials = new NetworkCredential(_settings.FromEmail, _settings.AppPassword)

            };

        }

        public EmailSettings Get_settings()
        {
            return _settings;
        }

        public async Task SendSimpleEmailAsync(string toEmail, string subject, string body)

        {

            var message = new MailMessage(_settings.FromEmail, toEmail, subject, body);

            await CreateSmtpClient().SendMailAsync(message);

        }
    }
}
