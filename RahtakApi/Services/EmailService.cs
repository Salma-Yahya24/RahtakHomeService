using RahtakApi.Entities.Interfaces;
using System.Net.Mail;
using System.Net;

namespace RahtakApi.Services;

public class EmailService : IEmailService
{
    public async Task<bool> SendEmailAsync(string userEmail, string userPassword, string to, string subject, string body)
    {
        try
        {
            string smtpServer = "smtp.gmail.com";
            int port = 587;

            using (SmtpClient client = new SmtpClient(smtpServer, port))
            {
                client.Credentials = new NetworkCredential(userEmail, userPassword);
                client.EnableSsl = true;

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(userEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mail.To.Add(to);

                await client.SendMailAsync(mail);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email sending failed: {ex.Message}");
            return false;
        }
    }
}
