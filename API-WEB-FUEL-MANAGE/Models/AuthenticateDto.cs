using System.ComponentModel.DataAnnotations;

namespace API_WEB_FUEL_MANAGE.Models
{
    public class AuthenticateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
