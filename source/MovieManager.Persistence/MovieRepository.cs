using Microsoft.EntityFrameworkCore;
using MovieManager.Core.Contracts;
using MovieManager.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Persistence
{
  public class MovieRepository : IMovieRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public MovieRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public void Delete(Movie movie)
      => _dbContext.Movies.Remove(movie);

    /// <summary>
    /// Liefert alle Filme zu einer übergebenen Kategorie sortiert nach Titel
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Movie[]> GetAllByCatIdAsync(int id)
      => await _dbContext.Movies
            .Where(m => m.CategoryId == id)
            .OrderBy(m => m.Title)
            .ToArrayAsync();

    /// <summary>
    /// Liefert den Film mit der übergebenen Id (null wenn nicht gefunden)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Movie> GetByIdAsync(int id)
      => await _dbContext.Movies
            .SingleOrDefaultAsync(m => m.Id == id);

    /// <summary>
    /// Liefert die Anzahl aller Filme in der Datenbank
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetCountAsync()
      => await _dbContext.Movies
          .CountAsync();

    /// <summary>
    /// Liefert den Film mit der längsten Dauer
    /// </summary>
    /// <returns></returns>
    public async Task<Movie> GetLongestMovieAsync()
      => await _dbContext.Movies
          .OrderByDescending(m => m.Duration)
          .ThenBy(m => m.Title)
          .FirstAsync();

    public async Task AddRangeAsync(Movie[] movies)
      => await _dbContext.Movies.AddRangeAsync(movies);

    public async Task<Movie[]> GetAllAsync()
      => await _dbContext.Movies
           .OrderBy(m => m.Title)
           .ToArrayAsync();

    public async Task AddAsync(Movie movie)
      => await _dbContext.Movies.AddAsync(movie);

    /// <summary>
    /// Fügt neuen Film in der Datenbank hinzu
    /// </summary>
    /// <param name="movie"></param>
    public async Task InsertAsync(Movie movie)
      => await _dbContext.Movies.AddAsync(movie);
  }
}