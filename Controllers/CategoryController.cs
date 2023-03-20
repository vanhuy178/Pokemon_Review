using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System.Runtime.InteropServices;

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
        public async Task<ActionResult> GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(await _categoryRepositoty.GetCategories());

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

        public async  Task<ActionResult> GetCategory(int categoryId)
            
        {
            var value = await _categoryRepositoty.CategoryExist(categoryId);
            if ( !value) {
                return NotFound();
            }
            var cartegory = _mapper.Map<PokemonDTO>(await _categoryRepositoty.GetCategory(categoryId));
            if(!ModelState.IsValid) { 
                return BadRequest(ModelState);  
            }
            return Ok(cartegory);

        }

        [HttpGet("pokemon/{categoryId}")]

        public async Task<ActionResult> GetPokemonByCategoryId(int categoryId) {
            var value = await _categoryRepositoty.CategoryExist(categoryId);
            if (!value)
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<List<PokemonDTO >>(
                await _categoryRepositoty.GetPokemonByCategories(categoryId));
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
        public async Task<ActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO) {

            if(categoryDTO == null) {
                return BadRequest(ModelState);
            }

            var value = await _categoryRepositoty.GetCategories();
            var category = value.Where(c => c.Name.Trim().ToUpper() == categoryDTO.Name.ToUpper()).FirstOrDefault();  

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

            var valueCategoryMap = await _categoryRepositoty.CreateCategory(categoryMap);
            if (!valueCategoryMap)
            {
                ModelState.AddModelError("", "Something was wrong while saving");
                return StatusCode(500, categoryMap);
            }
            return Ok("Create successfully");    
        }

        [HttpPut("categoryId")]
        public async Task<IActionResult> UpdatedCategory(int categoryId, [FromBody] CategoryDTO updateCategory)
        {   
            if(updateCategory == null) {
                return BadRequest(ModelState);
            }

            if(categoryId != updateCategory.Id) {
                return BadRequest(ModelState);
            }
            var value = await _categoryRepositoty.CategoryExist(categoryId);
            if (!value) {
                return NotFound();
            }
            if(!ModelState.IsValid) {
                return BadRequest();        
            }

            var categoryMap = _mapper.Map<Category>(updateCategory);
            var valueCategoryMap =await _categoryRepositoty.UpdateCategory(categoryMap);
            if (!valueCategoryMap)
            {
                ModelState.AddModelError("", "Some thing was wrong !!!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [HttpDelete("{categoryId}")]

        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            var value = await _categoryRepositoty.CategoryExist(categoryId);
            if (!value) {
                return NotFound();
            }

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var categoryDelete = await _categoryRepositoty.GetCategory(categoryId);
            var valueCategory = await _categoryRepositoty.DeleteCategory(categoryDelete);

            if (!valueCategory)
            {
                ModelState.AddModelError("", "Something was wrong when deleteing");
            }

            return NoContent(); 
        }

    }    
}
