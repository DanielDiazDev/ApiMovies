using ApiMovies.Data;
using ApiMovies.Models;
using ApiMovies.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool AddMovie(Movie movie)
        {
            movie.CreationDate = DateTime.Now;
            _context.Movies.Add(movie);
            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _context.Movies.Remove(movie);
            return Save();
        }

        public ICollection<Movie> FindMovies(string name)
        {
            IQueryable<Movie> query = _context.Movies;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name) || x.Description.Contains(name));
            }
            return query.ToList();
        }

        public Movie GetMovie(int movieId)
        {
            return _context.Movies.FirstOrDefault(c => c.Id == movieId);
        }

        public ICollection<Movie> GetMovies()
        {
            return _context.Movies.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Movie> GetMoviesInCategory(int catId)
        {
            return _context.Movies.Include(m  => m.Category).Where(m=>m.categoryId == catId).ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool HasMovie(string name)
        {
            return _context.Movies.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool HasMovie(int id)
        {
            return _context.Movies.Any(c => c.Id == id);
        }

        public bool UpdateMovie(Movie movie)
        {
            movie.CreationDate = DateTime.Now;
           _context.Movies.Update(movie);
            return Save();
        }
    }
}
