using Microsoft.EntityFrameworkCore;
using MovieManager.Core.Contracts;
using MovieManager.Core.DTOs;
using MovieManager.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Persistence
{
  internal class CategoryRepository : ICategoryRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    /// <summary>
    /// Liefert eine Liste aller Kategorien sortiert nach dem CategoryName
    /// </summary>
    /// <returns></returns>
    public async Task<Category[]> GetAllAsync()
      => await _dbContext.Categories
            .OrderBy(c => c.CategoryName)
            .ToArrayAsync();

    /// <summary>
    /// Liefert die Kategorie mit der übergebenen Id --> null wenn nicht gefunden
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Category> GetByIdAsync(int id)
      => await _dbContext.Categories
            .SingleOrDefaultAsync(c => c.Id == id);


    /// <summary>
    /// Liefert eine Liste mit der Anzahl und Gesamtdauer aller Filme je Kategorie
    /// Sortiert nach dem Namen der Kategorie (aufsteigend).
    /// </summary>
    public CategoryStatisticEntry[] GetCategoryStatistics()
      => _dbContext.Categories
            .Select(c =>
                    new CategoryStatisticEntry()
                    {
                      Category = c,
                      NumberOfMovies = c.Movies.Count(),
                      TotalDuration = c.Movies.Sum(m => m.Duration)
                    }
              )
           .OrderBy(entry => entry.Category.CategoryName)
           .ToArray();

    /// <summary>
    /// Liefert die Kategorie mit den meisten Filmen
    /// </summary>
    public CategoryStatisticEntry GetCategoryWithMostMovies()
      => _dbContext.Categories
            .Select(c =>
                    new CategoryStatisticEntry()
                    {
                      Category = c,
                      NumberOfMovies = c.Movies.Count(),
                      TotalDuration = c.Movies.Sum(m => m.Duration)
                    }
              )
           .OrderByDescending(entry => entry.NumberOfMovies)
           .ThenBy(entry => entry.Category.CategoryName)
           .FirstOrDefault();

    /// <summary>
    /// Liefert die Kategorien mit der durchschnittlichen Länge der zugeordneten Filme.
    /// Absteigend sortiert nach der durchschnittlichen Dauer der Filme - bei gleicher
    /// Dauer dann nach dem Namen der Kategorie aufsteigend. 
    /// </summary>
    public (Category Category, double AverageLength)[] GetCategoriesWithAverageLengthOfMovies()
      => _dbContext.Categories
          .Select(category =>
              ValueTuple.Create(
                  category,
                  category.Movies.Average(movie => movie.Duration)))
          .ToArray()
          .OrderByDescending(result => result.Item2)
          .ThenBy(result => result.Item1.CategoryName)
          .ToArray();

    public int GetYearWithMostPublicationsForCategory(string categoryName)
      => _dbContext.Movies
          .Where(movie => movie.Category.CategoryName == categoryName)
          .GroupBy(movie => movie.Year)
          .Select(movieGroupByYear =>
              new
              {
                Year = movieGroupByYear.Key,
                CntOfMovies = movieGroupByYear.Count()
              })
          .OrderByDescending(movieGroupByYear => movieGroupByYear.CntOfMovies)
          .First().Year;

    /// <summary>
    /// Neue Kategorie wird in Datenbank eingefügt
    /// </summary>
    /// <param name="category"></param>
    public async Task InsertAsync(Category category) => await _dbContext.Categories.AddAsync(category);

    /// <summary>
    /// Kategorie per Id laden.
    /// </summary>
    public Category GetByIdWithMovies(int id)
      => _dbContext.Categories
        .Include(c => c.Movies)
        .FirstOrDefault(c => c.Id == id);

    public void Remove(Category category) => _dbContext.Categories.Remove(category);
  }
}