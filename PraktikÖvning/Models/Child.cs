using Microsoft.AspNetCore.Identity;

namespace PraktikÖvning.Models
{
    public class Child
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        // Foreigen key for the parent user 

        public string? UserId { get; set; }

        public IdentityUser? User { get; set; }
    }
}
