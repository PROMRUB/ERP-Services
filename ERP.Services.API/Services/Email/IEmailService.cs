namespace ERP.Services.API.Services.Email
{
    using System.Text;
    using System.Threading.Tasks;
    using MimeKit.Text;
}

namespace User.Api.Application.Services
{
    public interface IEmailService
    {
        Task Send<T>(string to, string subject, string template, T data, string from = null);
    }
}