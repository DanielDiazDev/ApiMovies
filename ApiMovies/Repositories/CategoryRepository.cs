using ApiMovies.Data;
using ApiMovies.Models;
using ApiMovies.Repositories.Interfaces;

namespace ApiMovies.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(c=>c.Id == id);
        }

        public bool HasCategory(int id)
        {
            return _context.Categories.Any(c=> c.Id == id);
        }

        public bool HasCategory(string name)
        {
            return _context.Categories.Any(c=> c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool AddCategory(Category category)
        {
            category.CreatedDate = DateTime.Now;
            _context.Categories.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            category.CreatedDate = DateTime.Now;
            _context.Categories.Update(category);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

    }
}
