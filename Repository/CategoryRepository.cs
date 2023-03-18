using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context) {
            _context = context;
        }
        public bool CategoryExist(int id)
        {
            return _context.Categories.Any(item => item.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.Id).ToList();    
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(e => e.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategories(int categoryId)
        {
           return _context.PokemonCategories.Where(e => e.CategoryId  == categoryId).Select(e =>e.Pokemon).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges(); 

            return saved > 0 ? true : false;

        }
    }
}
    