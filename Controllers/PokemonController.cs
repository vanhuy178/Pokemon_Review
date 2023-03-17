using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper) {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
       

        public  ActionResult<Pokemon> GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pokemons == null)
            {
                return NotFound();
            }
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]

        public  IActionResult GetPokemon (int pokeId)
        {
            if(!_pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound(ModelState);

            }
                var pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemon (pokeId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(pokemon); 
        }

        [HttpGet("{pokeId}/rating")]
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
