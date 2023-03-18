using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interface;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly IContryRepository _countryRepository;

        public OwnerController(IOwnerRepository ownerRepository, IContryRepository countryRepository, IMapper mapper) {
            _ownerRepository = ownerRepository; 
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        public IActionResult GetOWners() 
        {
            var owners = _mapper.Map<List<OwnerDTO>>(_ownerRepository.GetOwners());
             
            if (owners == null)
            {
                return NotFound(owners);
            }

            if(!ModelState.IsValid) {
            }

            return Ok(owners);  
        }

        [HttpGet("{ownerId}")]
        public IActionResult GetOwner(int ownerId) {
           if(!_ownerRepository.OwnerExist(ownerId)) {
                return NotFound(ownerId);
            }

           var owner = _mapper.Map<OwnerDTO>(_ownerRepository.GetOwner(ownerId)); 

            if(owner == null) {
                return BadRequest(owner);
            }

            return Ok(owner);

        }
        [HttpGet("{ownerId}/pokemon")]

        public IActionResult GetPokemonByOwner (int ownerId) {
            if (!_ownerRepository.OwnerExist(ownerId))
            {
                return NotFound();
            }

            var owner = _mapper.Map<List<PokemonDTO>>(
                _ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);

        }

        [HttpPost]
        public IActionResult PostOwner([FromBody] OwnerDTO ownerDTO, [FromQuery] int countryId) {
            if (ownerDTO == null)
            {
                return BadRequest(ModelState);
            }
            

            var owner = _ownerRepository.GetOwners().Where(e => e.FirstName.Trim().ToUpper() == ownerDTO.FirstName.Trim().ToUpper()).FirstOrDefault();

            if(owner != null) {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(400, ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerDTO);
            ownerMap.Country = _countryRepository.GetCountry(countryId);
             

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");

        }
    }
}
