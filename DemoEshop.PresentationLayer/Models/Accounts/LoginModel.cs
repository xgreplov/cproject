using System.ComponentModel.DataAnnotations;

namespace DemoEshop.PresentationLayer.Models.Accounts
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}