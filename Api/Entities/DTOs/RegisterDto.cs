using System.ComponentModel.DataAnnotations;

namespace Api.Entities.DTOs
{
    public class RegisterDto
    {
        [Required] public string Username { get; set; }
        
        
        [StringLength(16,MinimumLength = 8)]
        [Required] public string Password { get; set; }
        [Required] public string KnownAs { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public DateTime? DateOfBirth { get; set; } //in case of DateOnly, if we didn't put ? then 
                                                              //required will initially set date to default date-time value
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }

    }
}