using ApplicationLayer.Interface;
using DomainLayer.Entities;
using System.Net.Mail;
using System.Net;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;
        private readonly string senderEmail;
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            smtpServer = _configuration["SmtpConfig:SmtpServer"];
            smtpPort = int.Parse(_configuration["SmtpConfig:SmtpPort"]);
            smtpUsername = _configuration["SmtpConfig:SmtpUsername"];
            smtpPassword = _configuration["SmtpConfig:SmtpPassword"];
            senderEmail = _configuration["SmtpConfig:SenderEmail"];
        }

        public bool SendEmail(string To, string subject, string emailBody)
        {
            try
            {
                using var smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
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
