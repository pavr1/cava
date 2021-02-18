using cava.Custom.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace cava.Custom.Notification
{
    public class EmailSender : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage mail = new MailMessage(WebConfigurationManager.AppSettings[CommonObjects._CAVA_ADMIN_EMAIL_KEY], message.Destination, message.Subject, message.Body);
            mail.IsBodyHtml = true;

            var client = new SmtpClient(WebConfigurationManager.AppSettings[CommonObjects._MAIL_HOST_KEY], Convert.ToInt32(WebConfigurationManager.AppSettings[CommonObjects._MAIL_PORT_KEY]))
            {
                Credentials = new NetworkCredential(WebConfigurationManager.AppSettings[CommonObjects._NOTIFICATIONS_USER_KEY], WebConfigurationManager.AppSettings[CommonObjects._NOTIFICATIONS_PASSWORD_KEY]),
                EnableSsl = false
            };

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                //log
            }

            return Task.FromResult(0);
        }
    }
}