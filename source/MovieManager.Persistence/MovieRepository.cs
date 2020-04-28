using MovieManager.Core.Contracts;
using MovieManager.Core.Entities;
using System.Linq;

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
    public Movie[] GetAllByCatId(int id)
      => _dbContext.Movies
            .Where(m => m.CategoryId == id)
            .OrderBy(m => m.Title)
            .ToArray();

    /// <summary>
    /// Liefert den Film mit der übergebenen Id (null wenn nicht gefunden)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Movie GetById(int id)
      => _dbContext.Movies
            .SingleOrDefault(m => m.Id == id);

    /// <summary>
    /// Liefert die Anzahl aller Filme in der Datenbank
    /// </summary>
    /// <returns></returns>
    public int GetCount()
      => _dbContext.Movies
          .Count();

    /// <summary>
    /// Liefert den Film mit der längsten Dauer
    /// </summary>
    /// <returns></returns>
    public Movie GetLongestMovie()
      => _dbContext.Movies
          .OrderByDescending(m => m.Duration)
          .ThenBy(m => m.Title)
          .First();

    public void AddRange(Movie[] movies)
      => _dbContext.Movies.AddRange(movies);

    public Movie[] GetAll()
      => _dbContext.Movies
           .OrderBy(m => m.Title)
           .ToArray();

    public void Add(Movie movie)
      => _dbContext.Movies.Add(movie);

    /// <summary>
    /// Fügt neuen Film in der Datenbank hinzu
    /// </summary>
    /// <param name="movie"></param>
    public void Insert(Movie movie)
      => _dbContext.Movies.Add(movie);
  }
}