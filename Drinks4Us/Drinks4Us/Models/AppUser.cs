using System;
using SQLite;

namespace Drinks4Us.Models
{
    [Table("app_users")]
    public class AppUser
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("profile_picture")]
        public string ProfilePicture { get; set; } = "default_profile.png";

        [Column("register_date")]
        public DateTime RegisterDate { get; set; }

        [Column("last_login")]
        public DateTime LastLogin { get; set; }

        public override string ToString() => Email;
    }
}