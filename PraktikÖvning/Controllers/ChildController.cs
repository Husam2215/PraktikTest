using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PraktikÖvning.Database;
using PraktikÖvning.Models;

namespace PraktikÖvning.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ChildController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("add-child")]
        public async Task<IActionResult> AddChild([FromBody] Child model)
        {
            // Kontrollerar om användaren är autentiserad
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Sätt UserId från den autentiserade användaren
            model.UserId = user.Id;

            // Lägg till barnet i databasen
            _context.Children.Add(model);
            await _context.SaveChangesAsync(); // Glöm inte att spara ändringarna i databasen

            return Ok(new { message = "Child added successfully" });
        }


    }
}
