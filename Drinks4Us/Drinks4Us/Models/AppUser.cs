namespace Drinks4Us.Models
{
    public class AppUser
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ProfilePicture { get; set; } = "default_profile.png";

        public override string ToString() => "AppUser{Id=" + Id + ", Email=" + Email + ", Password=" + Password + "}";
    }
}