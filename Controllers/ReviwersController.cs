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

        [HttpPost]
        public IActionResult CreateReviewer([FromBody] ReviwersDTO reviewersCreate) 
        {
            try
            {
                if (reviewersCreate == null)
                {
                    return BadRequest(ModelState);
                }
                var reviewes = _reviewerRepository.GetReviewers().Where(p => p.FirstName.Trim().ToUpper() == reviewersCreate.FirstName.Trim().ToUpper()).FirstOrDefault();
                if (reviewes != null)
                {
                    ModelState.AddModelError("", "Owner already exist");
                    return StatusCode(400, ModelState);
                }

                var reviewerMap = _mapper.Map<Reviewer>(reviewersCreate);
             


                if (!_reviewerRepository.CreateReviewer(reviewerMap))
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

        [HttpPut("{reviewerId}")]
        public IActionResult UpdateReviers(int reviewerId, ReviwersDTO reviwersDTO)
        {
            if(reviwersDTO == null)
            {
                return BadRequest(ModelState);  
            }

            if(reviewerId != reviwersDTO.Id) { 
                return BadRequest(ModelState);  
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviwersDTO);

            if(!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something was wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewerId}")]
        public IActionResult DeleteReviewer(int reviewerId )
        {
            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewerDelete = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewerRepository.DeleteReviewer(reviewerDelete))
            {
                ModelState.AddModelError("", "Something was wrong when deleteing");
            }

            return NoContent();
        }
    } 
}
