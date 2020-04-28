using System;

namespace MovieManager.Core.Contracts
{
    public interface IUnitOfWork : IDisposable
    {

        IMovieRepository MovieRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        void Save();

        void DeleteDatabase();

        void MigrateDatabase();
    }
}
