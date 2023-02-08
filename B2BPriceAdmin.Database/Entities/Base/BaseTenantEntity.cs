namespace B2BPriceAdmin.Database.Entities
{
    public abstract class BaseTenantEntity : BaseEntity
    {

        public int TenantId { get; set; }
        public Tenant Tenants { get; set; }
    }
}
