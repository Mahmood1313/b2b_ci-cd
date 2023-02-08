using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2BPriceAdmin.Database.Entities
{
    public class AdmMenu : BaseEntity
    {
        [StringLength(24)]
        public string MenuId { get; set; }

        [ForeignKey(nameof(ParentMenuId))]
        public int ParentMenuId { get; set; }
        public AdmMenu ParentAdmMenu { get; set; }

        [Required]
        [StringLength(50)]
        public string MenuName { get; set; }
        [Column("arFormName")]
        [StringLength(50)]
        public string ArMenuName { get; set; }

        [Required]
        public bool ParentOnly { get; set; }

        [Required]
        public bool RootMenu { get; set; }

        [Required]
        public bool FnRead { get; set; }

        [Required]
        public bool FnAdd { get; set; }

        [Required]
        public bool FnWrite { get; set; }

        [Required]
        public bool FnDelete { get; set; }

        [Required]
        public bool FnClose { get; set; }

        [Required]
        public bool FnCancel { get; set; }

        [Required]
        public bool FnSubmit { get; set; }

        [Required]
        public bool FnApprove { get; set; }

        [Required]
        public bool FnPrint { get; set; }

        [Required]
        public bool PublicFlag { get; set; }

        [Required]
        public bool NotInMenu { get; set; }

        public int? SortOrder { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public new string CreatedBy { get; set; }

        [Required]
        public new DateTime CreatedOn { get; set; }
        public new string LastModifiedBy { get; set; }
        public new DateTime? LastModifiedOn { get; set; }
        public new DateTime? DeletedOn { get; set; }
        public new string DeletedBy { get; set; }

        [Required]
        public new bool Deleted { get; set; } = false;

        [Required]
        [Timestamp]
        public new byte[] Timestamp { get; set; }
    }
}
