using System.ComponentModel.DataAnnotations;

namespace ATF_Onliner.Models
{
    public class User
    {
         public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}