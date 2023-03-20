using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;
using System.Security.Cryptography.X509Certificates;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public  PokemonRepository(DataContext context) {
            _context = context; 
        }

        public async Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwenerEntity = await _context.Owners.Where(a => a.Id == ownerId).FirstOrDefaultAsync(); 
            var category = await _context.Categories.Where(a => a.Id == categoryId).FirstOrDefaultAsync();
            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwenerEntity,
                Pokemon = pokemon
            };

            await _context.AddAsync(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };

            await _context.AddAsync(pokemonCategory);

            await _context.AddAsync(pokemon);

            return await Save();
        }

        public async Task<Pokemon> GetPokemon(int id)
        {
           return await _context.Pokemon.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task <Pokemon> GetPokemon(string name)
        {
            return await  _context.Pokemon.Where(p => p.Name == name).FirstOrDefaultAsync();
        }

        public  async Task<decimal> GetPokemonRating(int pokemonId)
        {
            var review =  _context.Reviews.Where(p => p.Pokemon.Id == pokemonId);
            if (review.Count() < 0)
            {
                return 0;
            }
            return (await review.SumAsync(r => r.Rating) / review.Count());    
        }
          
        public async Task<ICollection<Pokemon>> GetPokemons()
        {
            return await  _context.Pokemon.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<bool> PokemonExists(int pokemonId)
        {
           return await _context.Pokemon.AnyAsync(p => p.Id == pokemonId);

        }

        public async Task<bool> DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return await Save();
        }       

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();

            return saved > 0 ? true : false;    
        }

        public async Task<bool> UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return await Save();
        }
    }
}
