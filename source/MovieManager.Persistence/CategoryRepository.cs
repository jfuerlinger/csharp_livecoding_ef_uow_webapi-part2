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
    public async Task<CategoryStatisticEntry[]> GetCategoryStatisticsAsync()
      => await _dbContext.Categories
            .Select(c =>
                    new CategoryStatisticEntry()
                    {
                      Category = c,
                      NumberOfMovies = c.Movies.Count(),
                      TotalDuration = c.Movies.Sum(m => m.Duration)
                    }
              )
           .OrderBy(entry => entry.Category.CategoryName)
           .ToArrayAsync();

    /// <summary>
    /// Liefert die Kategorie mit den meisten Filmen
    /// </summary>
    public async Task<CategoryStatisticEntry> GetCategoryWithMostMoviesAsync()
      => await _dbContext.Categories
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
           .FirstOrDefaultAsync();

    /// <summary>
    /// Liefert die Kategorien mit der durchschnittlichen Länge der zugeordneten Filme.
    /// Absteigend sortiert nach der durchschnittlichen Dauer der Filme - bei gleicher
    /// Dauer dann nach dem Namen der Kategorie aufsteigend. 
    /// </summary>
    public async Task<(Category Category, double AverageLength)[]> GetCategoriesWithAverageLengthOfMoviesAsync()
      => (await _dbContext.Categories
          .Select(category =>
              ValueTuple.Create(
                  category,
                  category.Movies.Average(movie => movie.Duration)))
          .ToArrayAsync())
          .OrderByDescending(result => result.Item2)
          .ThenBy(result => result.Item1.CategoryName)
          .ToArray();

    public async Task<int> GetYearWithMostPublicationsForCategoryAsync(string categoryName)
      =>  (await _dbContext.Movies
          .Where(movie => movie.Category.CategoryName == categoryName)
          .GroupBy(movie => movie.Year)
          .Select(movieGroupByYear =>
              new
              {
                Year = movieGroupByYear.Key,
                CntOfMovies = movieGroupByYear.Count()
              })
          .OrderByDescending(movieGroupByYear => movieGroupByYear.CntOfMovies)
          .FirstAsync()).Year;

    /// <summary>
    /// Neue Kategorie wird in Datenbank eingefügt
    /// </summary>
    /// <param name="category"></param>
    public async Task InsertAsync(Category category) => await _dbContext.Categories.AddAsync(category);

    /// <summary>
    /// Kategorie per Id laden.
    /// </summary>
    public async Task<Category> GetByIdWithMoviesAsync(int id)
      => await _dbContext.Categories
        .Include(c => c.Movies)
        .FirstOrDefaultAsync(c => c.Id == id);

    public void Delete(Category category) => _dbContext.Categories.Remove(category);
  }
}