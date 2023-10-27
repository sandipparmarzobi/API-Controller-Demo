using DomainLayer.Entities;
using URF.Core.Abstractions.Services;

namespace ApplicationLayer.Interface
{
    public interface IEmailService
    {
        bool SendEmail(string To, string CC, string subject, string emailBody);
    }
}