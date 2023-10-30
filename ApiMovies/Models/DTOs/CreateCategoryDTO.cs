using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Models.DTOs
{
    public class CreateCategoryDTO
    {
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(60, ErrorMessage = "El maximo de caracteres es de 100")]
        public string Name { get; set; }
    }
}
