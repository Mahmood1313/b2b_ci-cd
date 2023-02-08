namespace B2BPriceAdmin.Database.Entities
{
    public interface ISoftDelete
    {
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
    }
}
