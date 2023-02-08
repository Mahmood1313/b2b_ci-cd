using System.ComponentModel.DataAnnotations;

namespace B2BPriceAdmin.Database.Entities
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract class BaseEntity : IBaseEntity, IAuditableEntity, ISoftDelete, ITimeStamp
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        [Key]
        public int Id { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int? DeletedBy { get; set; }

        public bool Deleted { get; set; } = false;

        [Required]
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
