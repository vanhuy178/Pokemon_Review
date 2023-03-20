using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interface;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

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
        public async Task<ActionResult> GetOWners() 
        {
            var owners = _mapper.Map<List<OwnerDTO>>(await _ownerRepository.GetOwners());
             
            if (owners == null)
            {
                return NotFound(owners);
            }

            if(!ModelState.IsValid) {
            }

            return Ok(owners);  
        }

        [HttpGet("{ownerId}")]
        public async Task<ActionResult> GetOwner(int ownerId) {
           if(!await _ownerRepository.OwnerExist(ownerId)) {
                return NotFound(ownerId);
            }

           var owner = _mapper.Map<OwnerDTO>(await _ownerRepository.GetOwner(ownerId)); 

            if(owner == null) {
                return BadRequest(owner);
            }

            return Ok(owner);

        }
        [HttpGet("{ownerId}/pokemon")]

        public async Task<ActionResult> GetPokemonByOwner (int ownerId) {
            if (!await _ownerRepository.OwnerExist(ownerId))
            {
                return NotFound();
            }

            var owner = _mapper.Map<List<PokemonDTO>>(
                await _ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);

        }

        [HttpPost]
        public async Task<ActionResult> PostOwner([FromBody] OwnerDTO ownerDTO, [FromQuery] int countryId) {
            if (ownerDTO == null)
            {
                return BadRequest(ModelState);
            }
            var value = await _ownerRepository.GetOwners();

            var owner = value.Where(e => e.FirstName.Trim().ToUpper() == ownerDTO.FirstName.Trim().ToUpper()).FirstOrDefault();

            if(owner != null) {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(400, ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerDTO);
            ownerMap.Country = await _countryRepository.GetCountry(countryId);
            if (! await _ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something was wrong when saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{ownerId}")]
        public async Task<ActionResult> UpdateOwner(int ownerId, OwnerDTO ownerDTO) {
            if (ownerDTO == null)
            {
                return BadRequest(ModelState);
            }

            if(ownerId != ownerDTO.Id) {
                return BadRequest(ModelState);
            }

            if(!await _ownerRepository.OwnerExist(ownerId)) {
                return NotFound();
            }

            var ownerMap = _mapper.Map<Owner>(ownerDTO);
            if(!await _ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something was wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        public async Task<ActionResult> DeleteOwner(int ownerId) {
            if (!await _ownerRepository.OwnerExist(ownerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerDelete = await _ownerRepository.GetOwner(ownerId);

            if (!await _ownerRepository.DeleteOwner(ownerDelete))
            {
                ModelState.AddModelError("", "Something was wrong when deleteing");
            }

            return NoContent();

        }
    }
}
