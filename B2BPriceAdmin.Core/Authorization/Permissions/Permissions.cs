using System.ComponentModel.DataAnnotations;

namespace B2BPriceAdmin.Core.Authorization
{
    public enum Permissions : short
    {
        #region Super Admin Permission, Super Admin has control over everything

        [Display(GroupName = "Super Admin", Name = "All", Description = "Has authority over everything")]
        SuperAdminAll = 10,
        #endregion

        [Display(GroupName = "User Management", Name = "View", Description = "Can view all Users")]
        StaffView = 20,
        [Display(GroupName = "User Management", Name = "Create", Description = "Can create a new User")]
        UserCreate = 21,
        [Display(GroupName = "User Management", Name = "Update", Description = "Can update a User")]
        StaffUpdate = 22,
        [Display(GroupName = "User Management", Name = "Delete", Description = "Can delete a User")]
        StaffDelete = 23,

        [Display(GroupName = "Roles Management", Name = "View", Description = "Can list Roles")]
        RoleRead = 30,
        [Display(GroupName = "Roles Management", Name = "Create", Description = "Can create a Role")]
        RoleCreate = 31,
        [Display(GroupName = "Roles Management", Name = "Update", Description = "Can update a Role")]
        RoleUpdate = 32,
        [Display(GroupName = "Roles Management", Name = "Delete", Description = "Can delete a Role")]
        RoleDelete = 33,
    }
}