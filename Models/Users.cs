using Microsoft.AspNetCore.Identity;

namespace servicesharing.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
