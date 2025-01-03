using System.Net.Mail;
using System.Net;

namespace Movie_Point.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("Ahmedelbelkasy25@gmail.com", "zbin lttd ghbe pnph")
            };

            return client.SendMailAsync(
                new MailMessage(from: "ahmedelbelkasy25@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                {
                    IsBodyHtml = true
                });
        }
    }





}
