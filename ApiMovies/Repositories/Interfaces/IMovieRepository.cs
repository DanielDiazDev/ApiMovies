using ApiMovies.Models;

namespace ApiMovies.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();
        Movie GetMovie(int movieId);
        bool HasMovie(string name);
        bool HasMovie(int id);
        bool AddMovie(Movie movie);
        bool UpdateMovie(Movie movie);
        bool DeleteMovie(Movie movie);

        //Métodos para buscar pelicualas en categoría y buscar película por nombre
        ICollection<Movie> GetMoviesInCategory(int catId);
        ICollection<Movie> FindMovies(string name);

        bool Save();
    }
}
