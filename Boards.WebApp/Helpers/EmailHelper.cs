using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Boards.WebApp.Helpers
{
    public class EmailHelper
    {
        public async static Task Send(string destinyemail, string destinyname, string subject, string plainTextContent, string htmlContent)
        {

            var apiKey = Environment.GetEnvironmentVariable(Constants.EMAIL_API_KEY_ENVIROMENTVARIABLE);
            var smtpClient = new SmtpClient(Constants.EMAIL_SMTP_SERVER)
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
            };
            smtpClient.Credentials = new NetworkCredential("margi.innovation@gmail.com", @"%`\q?-eS6W\=y3jU%`\q?-eS6W\=y3jU");

            smtpClient.Send(Constants.EMAIL_FROM, destinyemail, subject, htmlContent);
        }
    }
}
