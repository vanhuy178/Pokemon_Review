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

        public Pokemon GetPokemon(int id)
        {
           return  _context.Pokemon.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return  _context.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public  decimal GetPokemonRating(int pokemonId)
        {
            var review =  _context.Reviews.Where(p => p.Pokemon.Id == pokemonId);
            if (review.Count() < 0)
            {
                return 0;
            }
            return (review.Sum(r => r.Rating) / review.Count());    
        }
          
        public ICollection<Pokemon> GetPokemons()
        {
            return  _context.Pokemon.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int pokemonId)
        {
           return _context.Pokemon.Any(p => p.Id == pokemonId);

        }
    }
}
