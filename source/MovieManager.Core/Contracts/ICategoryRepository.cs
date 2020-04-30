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
    CategoryStatisticEntry[] GetCategoryStatistics();
    CategoryStatisticEntry GetCategoryWithMostMovies();
    (Category Category, double AverageLength)[] GetCategoriesWithAverageLengthOfMovies();
    int GetYearWithMostPublicationsForCategory(string categoryName);
    Category GetByIdWithMovies(int id);
    void Remove(Category category);
  }
}
