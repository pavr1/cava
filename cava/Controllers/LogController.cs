using cava.Custom.Common;
using cava.Custom.Notification;
using cava.Models;
using Microsoft.AspNet.Identity;
using System.Web.Configuration;
using System.Web.Mvc;

namespace cava.Controllers
{
    public class LogController : Controller
    {
        public async void LogMessageAsync(Log log)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Logs.Add(log);

                await db.SaveChangesAsync();

                if (log.MessageType == CommonObjects._ERROR)
                {
                    var msg = new IdentityMessage()
                    {
                        Subject = string.Format(CommonObjects._NEW_ERROR_SUBJECT2 + log.LogGuid),
                        Destination = WebConfigurationManager.AppSettings[CommonObjects._CONFIG_ADMIN_EMAIL_KEY],
                        Body = string.Format(CommonObjects._ERROR_EMAIL_BODY, log.LogGuid, log.MessageDetail)
                    };

                    await new EmailSender().SendAsync(msg);
                }
            }
        }
    }
}