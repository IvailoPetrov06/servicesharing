using Microsoft.AspNetCore.Identity;

namespace servicesharing.Data.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

    }
}
