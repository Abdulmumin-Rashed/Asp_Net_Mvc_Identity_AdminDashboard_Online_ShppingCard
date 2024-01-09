using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class AppUser: IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? UrlImage { get; set; }
        [NotMapped]
        public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
