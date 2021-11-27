using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RegisterDto
    {
        //ON DTOS we can do some validation
        //Data coming from the frontend
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }   
        [Required]
        public string Password { get; set; }
    }
}
