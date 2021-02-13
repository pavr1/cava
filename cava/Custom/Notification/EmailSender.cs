using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace cava.Custom.Notification
{
    public class EmailSender : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage mail = new MailMessage(WebConfigurationManager.AppSettings["NoReplyEmail"], message.Destination, message.Subject, message.Body);
            mail.IsBodyHtml = true;

            var client = new SmtpClient(WebConfigurationManager.AppSettings["mailhost"], Convert.ToInt32(WebConfigurationManager.AppSettings["mailport"]))
            {
                Credentials = new NetworkCredential(WebConfigurationManager.AppSettings["NotificationsUser"], WebConfigurationManager.AppSettings["notificationspassword"]),
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