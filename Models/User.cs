using System.ComponentModel.DataAnnotations;

namespace AuthProject.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Email")]
        public string? Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "The Password field must be at least 6 characters long")]
        public string? Password { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "The ConfirmPassword field must be at least 6 characters long")]
        public string? ConfirmPassword { get; set; }

        
    }
}
