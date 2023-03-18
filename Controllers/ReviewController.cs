    using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviweRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviweRepository reviweRepository, IMapper mapper, IReviweRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            
        }


        [HttpGet]

        public IActionResult GetReviews()
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            var reviews =  _reviewRepository.GetReviews();

            if(reviews == null)
            {
                return NotFound();  
            }

            return Ok(reviews);  
        }

        [HttpGet("{reviewId}")]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExist(reviewId))
            {
                return NotFound(ModelState);

            }
            var pokemon = _mapper.Map<ReviewDTO>(_reviewRepository.GetReview(reviewId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }

        [HttpGet("pokemon/{pokeId}")]
        public IActionResult GetReviewsForPokemon(int pokeId)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var review = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviewOfPokemon(pokeId));    

            if(review == null)
            {

                return NotFound(review);
            }

            return Ok(review);  
        }
    }
}
