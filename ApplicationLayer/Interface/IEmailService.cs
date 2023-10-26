using DomainLayer.Entities;
using URF.Core.Abstractions.Services;

namespace ApplicationLayer.Interface
{
    public interface IEmailService
    {
        bool SendEmail(string To, string subject, string emailBody);
    }
}