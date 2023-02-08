using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2BPriceAdmin.Database.Entities
{
    public class Tenant : BaseEntity
    {
        [StringLength(64)]
        public string EnShortName { get; set; }

        [Required]
        [StringLength(128)]
        public string EnTenantName { get; set; }

        [StringLength(64)]
        public string ArShortName { get; set; }

        [StringLength(128)]
        public string ArTenantName { get; set; }

        [StringLength(2)]
        public string CountryCode { get; set; }

        public int CurrencyId { get; set; }

        [StringLength(128)]
        public string City { get; set; }

        [StringLength(128)]
        public string Street { get; set; }

        [StringLength(128)]
        public string POBox { get; set; }

        [StringLength(128)]
        public string Telephone { get; set; }

        [StringLength(128)]
        public string Email { get; set; }

        [Column("VAT", TypeName = "decimal(18, 4)")]
        public decimal? VAT { get; set; }

        [StringLength(128)]
        public string VATRef { get; set; }

        [Required]
        public bool Active { get; set; }

    }
}
