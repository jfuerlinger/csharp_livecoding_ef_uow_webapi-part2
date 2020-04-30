using System;
using System.Collections.Generic;
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
    public ActionResult<CategoryWithMoviesDto> GetById(int id)
    {
      Category category = _unitOfWork.Categories.GetByIdWithMovies(id);
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
    public ActionResult<MovieDto[]> GetMoviesByCategoryId(int id)
    {
      Category category = _unitOfWork.Categories.GetByIdWithMovies(id);
      if (category == null)
      {
        return NotFound();
      }

      return Ok(new CategoryWithMoviesDto(category).Movies);
    }

    /// <summary>
    /// Speichert eine neue Kategorie.
    /// </summary>
    /// <response code="201">Kategorie wurde erfolgeich erstellt.</response>
    /// <response code="400">Übergebene Kategorie konnte nicht erstellt werden.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddCategory(CategoryDto categoryDto)
    {
      Category category = new Category() { CategoryName = categoryDto.CategoryName};
      await _unitOfWork.Categories.InsertAsync(category);

      try
      {
        await _unitOfWork.SaveChangesAsync();
      } catch(ValidationException ex)
      {
        return BadRequest(ex.Message);
      }

      return CreatedAtAction(
        nameof(GetById), 
        new { id = category.Id }, 
        new CategoryDto() { CategoryName = category.CategoryName });
    }



    /// <summary>
    /// Ändert eine bestehende Kategorie.
    /// </summary>
    /// <response code="204">Kategorie wurde erfolgeich geändert.</response>
    /// <response code="400">Übergebene Name konnte nicht erfasst werden.</response>
    /// <response code="404">Kategorie mit der übergebenen id konnte nicht gefunden werden.</response>
    // PUT api/categories/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateCategory(int id, string categoryName)
    {
      var categoryInDb = await _unitOfWork.Categories.GetByIdAsync(id);
      if(categoryInDb == null)
      {
        return NotFound();
      }

      categoryInDb.CategoryName = categoryName;

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
    /// Löscht eine bestehende Kategorie.
    /// </summary>
    /// <response code="204">Kategorie wurde erfolgeich gelöscht.</response>
    /// <response code="400">Übergebene Name konnte nicht erfasst werden.</response>
    /// <response code="404">Kategorie mit der übergebenen id konnte nicht gefunden werden.</response>
    // DELETE api/categories/{id}
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

      _unitOfWork.Categories.Remove(categoryInDb);

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
