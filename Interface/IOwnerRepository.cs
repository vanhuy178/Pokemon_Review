using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IOwnerRepository
    {
         ICollection<Owner> GetOwners();
         Owner GetOwner(int ownerId);
         ICollection<Owner> GetOwnerOfPokemon (int ownerId);  
         ICollection<Pokemon> GetPokemonByOwner(int ownerId);
         bool OwnerExist(int ownerId);
         bool CreateOwner (Owner owner);
         bool Save();
    }
} 
    