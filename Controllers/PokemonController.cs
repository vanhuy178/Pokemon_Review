using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;

        public PokemonController(IPokemonRepository pokemonRepository) {
            _pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]

        public  ActionResult<Pokemon> GetPokemons()
        {
            var pokemon = _pokemonRepository.GetPokemons();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pokemon == null)
            {
                return NotFound();
            }
            return Ok(pokemon);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public  IActionResult GetPokemon (int pokeId)
        {
            if(!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound(ModelState);

            }
                var pokemon = _pokemonRepository.GetPokemon (pokeId);
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(pokemon); 
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if(!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }

            var rating = _pokemonRepository.GetPokemonRating (pokeId);  

            if(!ModelState.IsValid) {

                return BadRequest();

            }

            return Ok(rating);
        }

    }
}
