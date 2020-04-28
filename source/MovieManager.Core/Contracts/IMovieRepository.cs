using MovieManager.Core.Entities;
using System.Threading.Tasks;

namespace MovieManager.Core.Contracts
{
  public interface IMovieRepository
  {
    Task<int> GetCountAsync();
    Task<Movie[]> GetAllByCatIdAsync(int id);
    Task InsertAsync(Movie movie);
    Task<Movie> GetByIdAsync(int id);
    void Delete(Movie movie);
    Task<Movie> GetLongestMovieAsync();
    Task AddRangeAsync(Movie[] movies);
    Task<Movie[]> GetAllAsync();
    Task AddAsync(Movie movie);
  }
}
