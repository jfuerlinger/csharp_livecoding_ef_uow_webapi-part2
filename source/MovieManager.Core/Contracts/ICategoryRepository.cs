using MovieManager.Core.DTOs;
using MovieManager.Core.Entities;
using System.Threading.Tasks;

namespace MovieManager.Core.Contracts
{
  public interface ICategoryRepository
  {
    Task<Category[]> GetAllAsync();
    Task InsertAsync(Category category);
    Task<Category> GetByIdAsync(int id);
    Task<CategoryStatisticEntry[]> GetCategoryStatisticsAsync();
    Task<CategoryStatisticEntry> GetCategoryWithMostMoviesAsync();
    Task<(Category Category, double AverageLength)[]> GetCategoriesWithAverageLengthOfMoviesAsync();
    Task<int> GetYearWithMostPublicationsForCategoryAsync(string categoryName);
    Task<Category> GetByIdWithMoviesAsync(int id);
    void Delete(Category category);
  }
}
