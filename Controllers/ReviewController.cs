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
    public class ReviewController : ControllerBase
    {
        private readonly IReviweRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(IPokemonRepository pokemonRepository,
            IMapper mapper, IReviweRepository reviewRepository,
            IReviewerRepository reviewerRepository
            ) 
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]

        public async Task<ActionResult> GetReviews()
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            var reviews = _mapper.Map<List<ReviewDTO>>(await _reviewRepository.GetReviews());

            if(reviews == null)
            {
                return NotFound();  
            }

            return Ok(reviews);  
        }

        [HttpGet("{reviewId}")]
        public async Task<ActionResult> GetReview(int reviewId)
        {
            if (!await _reviewRepository.ReviewExist(reviewId))
            {
                return NotFound(ModelState);

            }
            var pokemon = _mapper.Map<ReviewDTO>(await _reviewRepository.GetReview(reviewId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpGet("pokemon/{pokeId}")]
        public async Task<ActionResult> GetReviewsForPokemon(int pokeId)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var review = _mapper.Map<List<ReviewDTO>>(await _reviewRepository.GetReviewOfPokemon(pokeId));    

            if(review == null)
            {

                return NotFound(review);
            }

            return Ok(review);  
        }

        [HttpPost]
        public async Task<ActionResult> CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId,[FromBody] ReviewDTO reviewDTO)
        {
            try
            {
                if (reviewDTO == null)
                {
                    return BadRequest(ModelState);
                }
                var value = await _reviewRepository.GetReviews();
                var reviews = value.Where(p => p.Title.Trim().ToUpper() == reviewDTO.Title.Trim().ToUpper()).FirstOrDefault();
                if (reviews != null)
                {
                    ModelState.AddModelError("", "Owner already exist");
                    return StatusCode(400, ModelState);
                }

                var reviewMap = _mapper.Map<Review>(reviewDTO);
                reviewMap.Pokemon = await _pokemonRepository.GetPokemon(pokeId);
                reviewMap.Reviewer = await _reviewerRepository.GetReviewer(reviewerId);


                if (! await _reviewRepository.CreateReview(reviewMap))    
                {

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

        [HttpPut("{reviewId}")]
        public async Task<ActionResult> UpdateReview(int reviewId, ReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewId != reviewDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var reviewMap = _mapper.Map<Review>(reviewDTO);
            if(!await _reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something was wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();     
        }

        [HttpDelete("{reviewId}")]
        public async Task<ActionResult> DeleteReview(int reviewId)
        {
            if (!await _reviewRepository.ReviewExist(reviewId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewDelete = await _reviewRepository.GetReview(reviewId);

            if (!await _reviewRepository.DeleteReview(reviewDelete))
            {
                ModelState.AddModelError("", "Something was wrong when deleteing");
            }

            return NoContent();
        }


    }
}
