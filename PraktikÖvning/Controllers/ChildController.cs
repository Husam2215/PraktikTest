using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiseKidBackend.Database;
using WiseKidBackend.Models;
using System.Security.Claims;

namespace WiseKidBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public ChildController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("add-child")]
        public async Task<IActionResult> AddChild([FromBody] Child model)
        {
            // Check if the user is authenticated
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }

            // Assign the current user's ID to the Child model
            model.UserId = user.Id;

            // Add the child to the database
            _context.Children.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Child added successfully", childId = model.Id });
        }

        [HttpDelete("deleteChild/{childId}")]
        [Authorize] // Se till att denna metod kräver autentisering
        public async Task<IActionResult> DeleteChild(int childId)
        {
            // Hämta användar-ID från den autentiserade användaren
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            // Hitta barnet i databasen
            var child = await _context.Children.FirstOrDefaultAsync(c => c.Id == childId);

            if (child == null)
            {
                return NotFound(new { message = "Child not found" });
            }

            if (child.UserId != userId) // Kontrollera att barnet är kopplat till den inloggade användaren
            {
                // Använd StatusCode för att ange en 403 Forbidden
                return StatusCode(403, new { message = "You don't have the right to delete that child" });
            }

            _context.Children.Remove(child);
            await _context.SaveChangesAsync(); // Använd async för att undvika blockering

            return Ok(new { message = "Child deleted successfully" });
        }

    }
}
