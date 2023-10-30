using ApiMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) //Heredamos en el construcotr el dbcontext para poder utilizarlo
        { 
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
