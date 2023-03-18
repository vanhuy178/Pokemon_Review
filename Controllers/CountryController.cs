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

        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDTO>>( _countryRepository.GetCountries());
                       
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        public IActionResult GetCountry(int countryId)
        {
            if(!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var country = _mapper.Map<CountryDTO>(_countryRepository.GetCountry(countryId));

            if(!ModelState.IsValid) { 
                return BadRequest(ModelState);  
            }       

            return Ok(country); 
        }

        [HttpGet("/owners/{ownerId}")]

        public IActionResult GetCountryOfAnOwner (int ownerId) {
            var country = _mapper.Map<CountryDTO>(_countryRepository.GetCountryByOwner(ownerId));

            if(!ModelState.IsValid) {
            
                return BadRequest();    
            }

            return Ok(country );
        
        }

        [HttpPost]
        public  IActionResult PostCountry( [FromBody] CountryDTO countryDTO) {
            if(countryDTO == null) {

                return BadRequest(ModelState);
            }

            var country = _countryRepository.GetCountries().Where(e => e.Name.Trim().ToUpper() == countryDTO.Name.Trim().ToUpper()).FirstOrDefault();
            
            if(country != null) {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(400, ModelState);
            } 

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            var countryMap = _mapper.Map<Country>(countryDTO);

            if(!_countryRepository.CreateCountry(countryMap)) {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }
    }
}
