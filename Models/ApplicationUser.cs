using Microsoft.AspNetCore.Identity;

namespace RESTaurant_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
