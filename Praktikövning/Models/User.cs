using Microsoft.AspNetCore.Identity;

namespace WiseKidBackend.Models
{
    public class User : IdentityUser
    {
        public ICollection<Child> Children { get; set; } = new List<Child>();
    }
}
