using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Core.Contracts;
using MovieManager.Core.DataTransferObjects;
using MovieManager.Core.Entities;

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
    public ActionResult<string[]> GetCategories()
    {
      return Ok(_unitOfWork
          .CategoryRepository
          .GetAll()
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
      Category category = _unitOfWork.CategoryRepository.GetByIdWithMovies(id);
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
      Category category = _unitOfWork.CategoryRepository.GetByIdWithMovies(id);
      if (category == null)
      {
        return NotFound();
      }

      return Ok(new CategoryWithMoviesDto(category).Movies);
    }
  }
}
