using Domain.Core.Models.Email;
using System.Threading.Tasks;

namespace Services.Contracts.Interfaces
{
    public interface IEmailSender
    {
        Task<EmailResponseModel> SendAsync(string toAddress, string subject, string body, string attachmentPath = null);
        EmailResponseModel Send(string toAddress, string subject, string body, string attachmentPath = null);
    }
}
