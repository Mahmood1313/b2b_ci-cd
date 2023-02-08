namespace B2BPriceAdmin.Database.Entities
{
    public interface IAuditableEntity
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
