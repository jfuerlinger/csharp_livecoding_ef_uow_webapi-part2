using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Core.Contracts;
using MovieManager.Core.DataTransferObjects;
using MovieManager.Core.Entities;
using MovieManager.Web.DataTransferObjects;

namespace MovieManager.Web.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {
    private readonly IUnitOfWork _unitOfWork;

    public CategoriesController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Liefert alle Kategorien.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<string[]>> GetCategories()
    {
      var categories = await _unitOfWork
          .Categories
          .GetAllAsync();

      return Ok(categories
          .Select(category => category.CategoryName)
          .ToArray());
    }

    /// <summary>
    /// Liefert eine Kategorie per Id.
    /// </summary>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryWithMoviesDto>> GetById(int id)
    {
      Category category = await _unitOfWork.Categories.GetByIdWithMoviesAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      return Ok(new CategoryWithMoviesDto(category));
    }


    /// <summary>
    /// Liefert die Movies zu einer Kategorie (per Id).
    /// </summary>
    [HttpGet]
    [Route("{id}/movies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieDto[]>> GetMoviesByCategoryId(int id)
    {
      Category category = await _unitOfWork.Categories.GetByIdWithMoviesAsync(id);
      if (category == null)
      {
        return NotFound();
      }

      return Ok(new CategoryWithMoviesDto(category).Movies);
    }

    /// <summary>
    /// Fügt eine neue Kategorie hinzu.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddCategory(CategoryDto categoryDto)
    {
      Category category = new Category() { CategoryName = categoryDto.CategoryName };
      await _unitOfWork.Categories.InsertAsync(category);

      try
      {
        await _unitOfWork.SaveChangesAsync();
      }
      catch (ValidationException ex)
      {
        return BadRequest(ex.Message);
      }

      return CreatedAtAction(
        nameof(GetById),
        new { id = category.Id },
        category);
    }

    /// <summary>
    /// Ändert eine bestehende Kategorie
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateCategory(int id, CategoryDto categoryDto)
    {
      Category categoryFromDb = await _unitOfWork
        .Categories
        .GetByIdAsync(id);

      if (categoryFromDb == null)
      {
        return NotFound();
      }

      categoryFromDb.CategoryName = categoryDto.CategoryName;

      try
      {
        await _unitOfWork.SaveChangesAsync();
      }
      catch (ValidationException ex)
      {
        return BadRequest(ex.Message);
      }

      return NoContent();
    }


    /// <summary>
    /// Löscht eine bestehende Kategorie
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCategory(int id)
    {
      var categoryInDb = await _unitOfWork.Categories.GetByIdAsync(id);
      if (categoryInDb == null)
      {
        return NotFound();
      }

      _unitOfWork.Categories.Delete(categoryInDb);

      try
      {
        await _unitOfWork.SaveChangesAsync();
      }
      catch (ValidationException ex)
      {
        return BadRequest(ex.Message);
      }

      return NoContent();
    }
  }
}
