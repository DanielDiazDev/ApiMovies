using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovies.Models.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        public string ImageRoute { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Description { get; set; }
        [Required(ErrorMessage = "La duracion es obligatoria")]

        public int Duration { get; set; }

        public enum ClasificationType { Seven, Thirteen, Seventeen, Eighteen }

        public ClasificationType Clasification { get; set; }
        public DateTime CreationDate { get; set; }

        public int categoryId { get; set; }
    }
}
