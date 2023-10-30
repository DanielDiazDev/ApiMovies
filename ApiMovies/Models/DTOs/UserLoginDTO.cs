using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.DTOs
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "El username es obligatorio")]
        public string Username { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
