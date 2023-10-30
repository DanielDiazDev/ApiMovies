using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovies.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ImageRoute { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }

        public enum ClasificationType { Seven, Thirteen, Seventeen, Eighteen }

        public ClasificationType Clasification { get; set; }
        public DateTime CreationDate { get; set; }

        [ForeignKey("categoryId")]
        public int categoryId { get; set; }
        public Category Category { get; set; }
    }
}

