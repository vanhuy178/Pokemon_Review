using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface ICategoryRepository
    {
         Task<ICollection<Category>> GetCategories();  
         Task<Category> GetCategory(int id);
         Task<ICollection<Pokemon>> GetPokemonByCategories(int id);
         Task<bool> CategoryExist(int id); 
         Task<bool> UpdateCategory(Category category); 
         Task<bool> CreateCategory(Category category);
         Task<bool> DeleteCategory(Category  category);    
         Task<bool> Save();
    }
}
