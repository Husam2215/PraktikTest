using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WiseKidBackend.Database;
using WiseKidBackend.Models;

namespace WiseKidBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        // Konstruktor för att sätta beroenden
        public ChildController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Lägg till ett barn
        [HttpPost("add-child")]
        public async Task<IActionResult> AddChild([FromBody] Child model)
        {
            // Kontrollera att användaren är inloggad
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }

            // Sätt användarens ID på barnet
            model.UserId = user.Id;

            // Lägg till barnet i databasen
            _context.Children.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Child added successfully", childId = model.Id });
        }

        // Ta bort ett barn
        [HttpDelete("deleteChild/{childId}")]
        [Authorize]
        public async Task<IActionResult> DeleteChild(int childId)
        {
            // Hämta användarens ID
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

            // Kontrollera att barnet tillhör den inloggade användaren
            if (child.UserId != userId)
            {
                return StatusCode(403, new { message = "You don't have the right to delete that child" });
            }

            // Ta bort barnet
            _context.Children.Remove(child);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Child deleted successfully" });
        }

    }
}
