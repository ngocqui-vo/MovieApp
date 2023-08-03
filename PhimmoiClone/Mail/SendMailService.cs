using MimeKit;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace PhimmoiClone.Mail
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class SendMailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<SendMailService> _logger;
        public SendMailService(IOptions<MailSettings> mailSettings, ILogger<SendMailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
            _logger.LogInformation("create SendMailService");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            }
            catch (Exception e)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await message.WriteToAsync(emailsavefile);

                _logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                _logger.LogError(e.Message);
            }

            smtp.Disconnect(true);

            _logger.LogInformation("send mail to: " + email);

        }
    }
}
