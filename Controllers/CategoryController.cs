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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepositoty;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper) { 
            _categoryRepositoty = categoryRepository;
            _mapper = mapper;
         }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepositoty.GetCategories());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]

        public IActionResult GetCategory(int categoryId)
        {
            if(!_categoryRepositoty.CategoryExist(categoryId)) {
                return NotFound();
            }
            var cartegory = _mapper.Map<PokemonDTO>(_categoryRepositoty.GetCategory(categoryId));
            if(!ModelState.IsValid) { 
                return BadRequest(ModelState);  
            }
            return Ok(cartegory);

        }

        [HttpGet("pokemon/{categoryId}")]

        public IActionResult GetPokemonByCategoryId(int categoryId) {
            if (!_categoryRepositoty.CategoryExist(categoryId))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<List<PokemonDTO >>(
                _categoryRepositoty.GetPokemonByCategories(categoryId));
            if(pokemon == null)
            {
                return BadRequest(pokemon);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemon);
        }


        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryDTO) {

            if(categoryDTO == null) {
                return BadRequest(ModelState);
            }
            var category =  _categoryRepositoty.GetCategories().Where(c => c.Name.Trim().ToUpper() == categoryDTO.Name.ToUpper()).FirstOrDefault();  

            if(category != null) {

                ModelState.AddModelError("", "Category already exist");
                return StatusCode(400, ModelState);
              };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // map categorydto ==> category 
            var categoryMap = _mapper.Map<Category>(categoryDTO);

            if(!_categoryRepositoty.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, categoryMap);
            }
            return Ok("Create successfully");    
        }
    }    
}
