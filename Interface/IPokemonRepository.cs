using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IPokemonRepository
    {
        Task<ICollection<Pokemon>> GetPokemons();
        
        Task<Pokemon> GetPokemon(int id);
        Task<Pokemon> GetPokemon(string name);
        Task<decimal> GetPokemonRating(int pokemonId);
        Task<bool> PokemonExists(int pokemonId);
        Task<bool> UpdatePokemon(Pokemon pokemon);
        Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        Task<bool> DeletePokemon(Pokemon pokemon);
        Task<bool> Save();
    }
}
