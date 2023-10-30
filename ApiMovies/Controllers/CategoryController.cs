using ApiMovies.Models;
using ApiMovies.Models.DTOs;
using ApiMovies.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiMovies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories()
        {
            var listCategories = _categoryRepository.GetCategories();
            var listCategoriesDto = new List<CategoryDTO>();
            foreach (var item in listCategories)
            {
                listCategoriesDto.Add(_mapper.Map<CategoryDTO>(item));
            }
            return Ok(listCategoriesDto);
        }
        [AllowAnonymous]
        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int categoryId)
        {
            var itemCategory = _categoryRepository.GetCategoryById(categoryId);

            if (itemCategory == null)
            {
                return NotFound();
            }

            var itemCategoryDto = _mapper.Map<CategoryDTO>(itemCategory);
            return Ok(itemCategoryDto);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(createCategoryDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (_categoryRepository.HasCategory(createCategoryDTO.Name))
            {
                ModelState.AddModelError("", "La categoría ya existe");
                return StatusCode(404, ModelState);
            }
            var category = _mapper.Map<Category>(createCategoryDTO);
            if (!_categoryRepository.AddCategory(category))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro{category.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategory", new { categoryId =  category.Id }, category);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPatch("{categoryId:int}", Name = "UpdatePatchCategory")]
        [ProducesResponseType(201, Type = typeof(CategoryDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePatchCategory(int categoryId, [FromBody] CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (categoryDTO == null || categoryId != categoryDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(categoryDTO);

            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro{category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.HasCategory(categoryId))
            {
                return NotFound();
            }

            var category = _categoryRepository.GetCategoryById(categoryId);

            if (!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro{category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
