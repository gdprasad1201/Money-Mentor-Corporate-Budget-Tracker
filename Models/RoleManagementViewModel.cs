using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Expense_Tracker.Models
{
    public class RoleManagementViewModel
    {
        public List<IdentityUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}
