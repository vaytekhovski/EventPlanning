using System;
using System.Net;
using System.Net.Mail;
using EventPlanning.Interfaces;

namespace EventPlanning.Services.Messages
{
    public class EmailMessageService : IMessageService
    {
        private readonly IConfiguration _configuration;

        public EmailMessageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync(string recipient, string messageBody)
        {
            var emailConfig = _configuration.GetSection("EmailSettings");

            using var mailMessage = new MailMessage()
            {
                From = new MailAddress(emailConfig["Sender"]),
                Subject = "Уведомление",
                Body = messageBody
            };
            mailMessage.To.Add(recipient);

            using var smtpClient = new SmtpClient(emailConfig["MailServer"])
            {
                Port = int.Parse(emailConfig["MailPort"]),
                Credentials = new NetworkCredential(emailConfig["Sender"], emailConfig["Password"]),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

