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
    public class ReviwersController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviwersController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper=mapper;
        }


        [HttpGet]
        public IActionResult GetReviewes()
        {
            var reviewers = _mapper.Map<List<ReviwersDTO>>(_reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reviewers == null)
            {
                return NotFound();
            }
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        public IActionResult GetReviewer(int reviewerId) {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewer = _mapper.Map<ReviwersDTO>(_reviewerRepository.GetReviewer(reviewerId));

            return Ok(reviewer);    
        }

        [HttpGet("{reviewerId}/reviews")]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var review = _mapper.Map<List<ReviewDTO>>(
            _reviewerRepository.GetReviewsByReviewer(reviewerId));

            if(review == null)
            {
                return BadRequest(review);
            }

            return Ok(review);
        }
    } 
}
