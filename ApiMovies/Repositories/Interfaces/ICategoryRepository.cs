using ApiMovies.Models;

namespace ApiMovies.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategoryById(int id);
        bool HasCategory(int id);
        bool HasCategory(string name);
        bool AddCategory(Category category);
        bool DeleteCategory(Category category);
        bool UpdateCategory(Category category);
        bool Save();
    }
}
