using System.ComponentModel.DataAnnotations;

namespace WiseKidBackend.Models
{
    public class Child
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }


        // Foreign key, primary key of the logged in user 
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
