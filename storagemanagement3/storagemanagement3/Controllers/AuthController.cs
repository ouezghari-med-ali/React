using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents;
using storagemanagement3.Services;
using Raven.Client.Documents.Session;
using storagemanagement3.Models;

namespace storagemanagement3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAsyncDocumentSession _session;

        public AuthController(RavenDbService ravenDbService)
        {
            _session = ravenDbService.DocumentStore.OpenAsyncSession();
        }

        [HttpPost("seed-user")]
        public async Task<IActionResult> SeedUser()
        {
            try
            {
                var user = new User
                {
                    Username = "admin",
                    PasswordHash = "XohImNooBHFRkE8y5rJ3nfsDNbZ4epIGI+WxnKJ8HXk=" // SHA-256 hash of "admin123"
                };

                await _session.StoreAsync(user);
                await _session.SaveChangesAsync();

                Console.WriteLine("User re-inserted successfully.");
                return Ok("User seeded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding user: {ex.Message}");
                return StatusCode(500, "Failed to seed user.");
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var user = await _session.Advanced
                .AsyncRawQuery<User>("from User where Username = $username")
                .AddParameter("username", loginRequest.Username)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                Console.WriteLine("User not found in the database.");
                return Unauthorized("Invalid username or password.");
            }

            Console.WriteLine($"User found: {user.Username}, Hash: {user.PasswordHash}");

            var passwordHash = HashPassword(loginRequest.Password);
            if (user.PasswordHash != passwordHash)
            {
                Console.WriteLine("Password hash does not match.");
                return Unauthorized("Invalid username or password.");
            }

            Console.WriteLine("Authentication successful.");
            return Ok("Authentication successful");
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
