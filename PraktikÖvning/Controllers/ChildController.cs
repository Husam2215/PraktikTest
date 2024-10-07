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
    }
}
