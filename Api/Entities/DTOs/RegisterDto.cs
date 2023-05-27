using System.ComponentModel.DataAnnotations;

namespace Api.Entities.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(16,MinimumLength = 8)]
        public string Password { get; set; }
    }
}