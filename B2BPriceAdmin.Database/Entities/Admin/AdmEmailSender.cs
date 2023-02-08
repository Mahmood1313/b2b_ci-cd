using System.ComponentModel.DataAnnotations;

namespace B2BPriceAdmin.Database.Entities
{
    /// <summary>
    /// Represents an email account
    /// </summary>
    public class AdmEmailSender : BaseTenantEntity
    {
        /// <summary>
        /// Gets or sets an email address
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets an email display name
        /// </summary>
        [Required]
        [StringLength(255)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets an email host
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets an email port
        /// </summary>
        [Required]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets an email user name
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets an email password
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value that controls whether the SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets a value that controls whether this use default credentials..
        /// </summary>
        public bool UseDefaultCredentials { get; set; }

        /// <summary>
        /// Gets or sets a value that controls whether this email is set as default sender.
        /// </summary>
        public bool UseDefault { get; set; } = false;
    }
}
