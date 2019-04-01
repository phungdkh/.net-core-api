using HVS.Api.Core.Business.Models;
using HVS.Api.Core.Entities;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace HVS.Api.Core.Business.Services
{
    public interface IEmailService
    {
        Task SendEmail(string subject, string htmlContent, User user);
    }

    public class EmailService : IEmailService
    {
        private readonly IOptions<AppSettings> _appSetting;

        public EmailService(IOptions<AppSettings> appSetting)
        {
            _appSetting = appSetting;
        }

        public async Task SendEmail(string subject, string htmlContent, User user)
        {
            var apiKey = _appSetting.Value.SendGridKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_appSetting.Value.EmailFrom, _appSetting.Value.FromName);
            var to = new EmailAddress(user.Email, user.Name);
            var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
