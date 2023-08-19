using System.ComponentModel.DataAnnotations;

namespace Task_Management_System.Models
{
    public class Login
    {
        [Required]
        [Display(Name = "Username")]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
    }
}
