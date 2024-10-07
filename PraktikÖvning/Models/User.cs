using Microsoft.AspNetCore.Identity;

namespace PraktikÖvning.Models
{
    public class User : IdentityUser
    {
        public ICollection<Child> Children { get; set; } = new List<Child>();
    }
}
