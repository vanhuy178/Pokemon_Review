using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface ICategoryRepository
    {
         ICollection<Category> GetCategories();  
         Category GetCategory(int id);
         ICollection<Pokemon> GetPokemonByCategories(int id);
        bool CategoryExist(int id); 
        bool CreateCategory(Category category);
        bool Save();
    }
}
