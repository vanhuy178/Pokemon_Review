using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly IContryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(IContryRepository countryRepository ,IMapper mapper) { 
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult> GetCountries()
        {
            var countries = _mapper.Map<List<CountryDTO>>(await _countryRepository.GetCountries());
                       
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        public async Task<ActionResult> GetCountry(int countryId)
        {
            var value = await _countryRepository.CountryExists(countryId);
            if (!value)
            {
                return NotFound();
            }

            var country = _mapper.Map<CountryDTO>(await _countryRepository.GetCountry(countryId));

            if(!ModelState.IsValid) { 
                return BadRequest(ModelState);  
            }       

            return Ok(country); 
        }

        [HttpGet("/owners/{ownerId}")]

        public async Task<ActionResult> GetCountryOfAnOwner (int ownerId) {
            var country = _mapper.Map<CountryDTO>(await _countryRepository.GetCountryByOwner(ownerId));

            if(!ModelState.IsValid) {
            
                return BadRequest();    
            }

            return Ok(country );
        
        }

        [HttpPost]
        public async  Task<ActionResult> PostCountry( [FromBody] CountryDTO countryDTO) {
            if(countryDTO == null) {

                return BadRequest(ModelState);
            }
            var value = await _countryRepository.GetCountries();
            var country = value.Where(e => e.Name.Trim().ToUpper() == countryDTO.Name.Trim().ToUpper()).FirstOrDefault();
            
            if(country != null) {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(400, ModelState);
            } 

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            var countryMap = _mapper.Map<Country>(countryDTO);
            var valueCountryMap = await _countryRepository.CreateCountry(countryMap);
            if (!valueCountryMap) {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{countryId}")]
        
        public async Task<ActionResult> UpdateCountry(int countryId,[FromBody] CountryDTO countryDTO) {
            if(countryDTO == null)
            {
                return BadRequest(ModelState);  
            }
            if(countryId != countryDTO.Id) {
                return BadRequest(ModelState);
            }

            if (!await _countryRepository.CountryExists(countryId))
            {
                return NotFound();  

            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var countryMap = _mapper.Map<Country>(countryDTO);
            if(!await _countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something was wrong");
                return StatusCode(500, ModelState);

            }
            return NoContent(); 

        }

        [HttpDelete("{countryId}")]
        public async Task<ActionResult> DeleteCategory(int countryId) {
            if (!await _countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryDelete = await _countryRepository.GetCountry(countryId);

            if (!await _countryRepository.DeleteCountry(categoryDelete))
            {
                ModelState.AddModelError("", "Something was wrong when deleteing");
            }

            return NoContent();
        }
    }
}
