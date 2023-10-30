using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "El username es obligatorio")]
        public string Username { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
