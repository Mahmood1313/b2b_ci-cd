namespace B2BPriceAdmin.Core.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="to">email addresses list</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="bcc">BCC addresses list</param>
        /// <param name="cc">CC addresses list</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public Task SendEmailAsync(List<string> to, string subject, string body, List<string> bcc = null, List<string> cc = null);
    }
}
