using System;
using System.Threading.Tasks;

namespace MovieManager.Core.Contracts
{
  public interface IUnitOfWork : IAsyncDisposable
  {

    IMovieRepository Movies { get; }
    ICategoryRepository Categories { get; }

    Task<int> SaveChangesAsync();
    Task DeleteDatabaseAsync();
    Task MigrateDatabaseAsync();
    Task CreateDatabaseAsync();
  }
}
