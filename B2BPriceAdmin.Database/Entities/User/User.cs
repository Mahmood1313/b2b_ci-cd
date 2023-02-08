using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace B2BPriceAdmin.Database.Entities
{
    public class User : IdentityUser<int>
    {
        public override int Id { get; set; }
        public override string UserName
        {
            get => base.Email;
            set => base.Email = value;
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public bool SuperUser { get; set; } = false;

        public bool Active { get; set; } = false;

        public int? TenantId { get; set; }

        [ForeignKey("TenantId")]
        public Tenant Tenants { get; set; }

        public int? CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [Required]
        public bool Deleted { get; set; } = false;
    }
}
