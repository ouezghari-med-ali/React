//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using Raven.Client.Documents;
//using Raven.Client.Documents.Session;
//using storagemanagement3.Models;

//namespace storagemanagement3.Services
//{

//    namespace storagemanagement3.Services
//    {
//        public class AuthenticationService
//        {
//            private readonly IAsyncDocumentSession _session;

//            public AuthenticationService(RavenDbService ravenDbService)
//            {
//                _session = ravenDbService.DocumentStore.OpenAsyncSession();
//            }

//            public async Task<bool> AuthenticateAsync(string username, string password)
//            {
//                // Fetch the single user from the database
//                var user = await _session.Query<User>().FirstOrDefaultAsync();

//                if (user == null || user.Username != username)
//                {
//                    return false; // User not found or username doesn't match
//                }

//                // Verify the password
//                var passwordHash = HashPassword(password);
//                return user.PasswordHash == passwordHash;
//            }

//            private string HashPassword(string password)
//            {
//                using var sha256 = SHA256.Create();
//                var bytes = Encoding.UTF8.GetBytes(password);
//                var hash = sha256.ComputeHash(bytes);
//                return Convert.ToBase64String(hash);
//            }
//        }
//    }

//}
