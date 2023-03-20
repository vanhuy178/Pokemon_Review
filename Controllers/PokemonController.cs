using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

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
       
        public async Task<ActionResult<Pokemon>> GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>( await _pokemonRepository.GetPokemons());

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

        public async  Task<ActionResult> GetPokemon (int pokeId)
        {
            if(!await _pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound(ModelState);

            }
                var pokemon = _mapper.Map<PokemonDTO>( await _pokemonRepository.GetPokemon (pokeId));
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            return Ok(pokemon); 
        }

        [HttpGet("{pokeId}/rating")]
        public async Task<ActionResult> GetPokemonRating(int pokeId)
        {
            if(!await _pokemonRepository.PokemonExists(pokeId))
            {
                return NotFound();
            }

            var rating = await _pokemonRepository.GetPokemonRating(pokeId);  
                
            if(!ModelState.IsValid) {

                return BadRequest();

            }

            return Ok(rating);
        }

        [HttpPost] 
        public async Task<ActionResult> CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryID, [FromBody] PokemonDTO pokemonCreate) {
            try
            {
                if(pokemonCreate == null) {
                    return BadRequest(ModelState);
                }
                var value = await _pokemonRepository.GetPokemons();
                var pokemons = value.Where(p => p.Name.Trim().ToUpper() == pokemonCreate.Name.Trim().ToUpper()).FirstOrDefault(); 
                if(pokemons != null)
                {
                    ModelState.AddModelError("", "Owner already exist");
                    return StatusCode(400, ModelState);
                }

                var pokemonsMap = _mapper.Map<Pokemon>(pokemonCreate);
                if(!await _pokemonRepository.CreatePokemon(ownerId, categoryID,pokemonsMap)) {

                    ModelState.AddModelError("", "Something was wrong");
                    return StatusCode(500, ModelState);
                }
                return Ok("Create sucessfully");
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error creating data from the database");
            }
        }

        [HttpPut("{pokemonId}")]

        public async Task<ActionResult> UpdatePokemon(int pokemonId, [FromBody] PokemonDTO pokemonDTO)
        {
            if (pokemonDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (pokemonId != pokemonDTO.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonDTO);
            if (!await _pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something was wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

        [HttpDelete("{pokemonId}")]
        public async  Task<ActionResult> DeletePokemon(int pokemonId) {
            if (!await _pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryDelete = await _pokemonRepository.GetPokemon(pokemonId);

            if (!await _pokemonRepository.DeletePokemon(categoryDelete));
            {
                ModelState.AddModelError("", "Something was wrong when deleteing");
            }

            return NoContent(); 
        }   

    }
}
