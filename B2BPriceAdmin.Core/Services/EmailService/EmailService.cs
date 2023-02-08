using B2BPriceAdmin.Core.Interfaces;
using B2BPriceAdmin.Database;

namespace B2BPriceAdmin.Core.Services
{
    public class EmailService : IEmailService
    {
        protected readonly B2BPriceDbContext _db;
        public EmailService(B2BPriceDbContext db)
        {
            _db = db;
        }
        public async Task SendEmailAsync(List<string> to, string subject, string body, List<string> bcc = null, List<string> cc = null)
        {
            //var emailHost = await _db.AdmEmailSenders.Where(a => a.UseDefault == true && a.Deleted == false).FirstOrDefaultAsync();

            //var email = new MailMessage
            //{
            //    // Email Content
            //    Sender = new MailAddress(emailHost.From, emailHost.DisplayName),
            //    Subject = subject,
            //    Body = body,
            //    IsBodyHtml = true
            //};

            //using SmtpClient smtp = new SmtpClient();
            //smtp.Host = emailHost.Smtpname;
            //smtp.Port = emailHost.Eport;
            //smtp.EnableSsl = emailHost.EnableSsl;
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.UseDefaultCredentials = emailHost.UseDefaultCredentials;
            //smtp.Credentials = new NetworkCredential(emailHost.FromEmail, emailHost.Password);

            //await smtp.SendMailAsync(email);
            //_db.Dispose();
        }
    }
}
