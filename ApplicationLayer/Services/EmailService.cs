using ApplicationLayer.Interface;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace ApplicationLayer.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;
        public EmailService(string smtpServer,int smtpPort,string smtpUsername, string smtpPassword,string senderEmail, ILogger<EmailService> logger)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;
            _senderEmail = senderEmail;
            _logger = logger;
        }


        public bool SendEmail(string To,string CC, string subject, string emailBody)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpServer, _smtpPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = subject,
                    Body = emailBody,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(To);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while sending the email.");
                return false;
            }
        }
    }
}
