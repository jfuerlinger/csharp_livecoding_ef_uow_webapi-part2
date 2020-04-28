using Microsoft.EntityFrameworkCore;
using MovieManager.Core.Contracts;
using System;

namespace MovieManager.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        const string Filename = "movies.csv";

        private readonly ApplicationDbContext _dbContext;
        private bool _disposed;


        public UnitOfWork()
        {
            _dbContext = new ApplicationDbContext();
            MovieRepository = new MovieRepository(_dbContext);
            CategoryRepository = new CategoryRepository(_dbContext);
        }

        public IMovieRepository MovieRepository { get; }

        public ICategoryRepository CategoryRepository { get; }


        /// <summary>
        ///     Repository-übergreifendes Speichern der Änderungen
        /// </summary>
        public void Save()
        {
            _dbContext.SaveChanges();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void DeleteDatabase()
        {
            _dbContext.Database.EnsureDeleted();
        }

        public void MigrateDatabase()
        {
            _dbContext.Database.Migrate();
        }

    }


}
