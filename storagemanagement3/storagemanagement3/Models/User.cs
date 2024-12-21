namespace storagemanagement3.Models
{
    public class User
    {
        public string Id { get; set; } // RavenDB assigns a unique ID
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

}
