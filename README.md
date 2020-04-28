# LiveCoding - WebApi Teil 2

## `UnitOfWork` (async)

```cs
public interface IUnitOfWork : IAsyncDisposable
{

  IMovieRepository Movies { get; }
  ICategoryRepository Categories { get; }

  Task<int> SaveChangesAsync();
  Task DeleteDatabaseAsync();
  Task MigrateDatabaseAsync();
  Task CreateDatabaseAsync();
}
```

```cs
public class UnitOfWork : IUnitOfWork
{
  private readonly ApplicationDbContext _dbContext;
  private bool _disposed;

  /// <summary>
  /// ConnectionString kommt aus den appsettings.json
  /// </summary>
  public UnitOfWork() : this(new ApplicationDbContext())
  {
  }

  public UnitOfWork(ApplicationDbContext dbContext)
  {
      _dbContext = dbContext;
      Movies = new MovieRepository(_dbContext);
      Categories = new CategoryRepository(_dbContext);
  }
  public IMovieRepository Movies { get; }
  public ICategoryRepository Categories { get; }


  public async Task<int> SaveChangesAsync()
  {
      var entities = _dbContext.ChangeTracker.Entries()
          .Where(entity => entity.State == EntityState.Added
                            || entity.State == EntityState.Modified)
          .Select(e => e.Entity)
          .ToArray();  // Geänderte Entities ermitteln
      foreach (var entity in entities)
      {
          var validationContext = new ValidationContext(entity, null, null);
          if (entity is IDatabaseValidatableObject)
          {     // UnitOfWork injizieren, wenn Interface implementiert ist
              validationContext.InitializeServiceProvider(serviceType => this);
          }

          var validationResults = new List<ValidationResult>();
          var isValid = Validator.TryValidateObject(entity, validationContext, validationResults,
              validateAllProperties: true);
          if (!isValid)
          {
              var memberNames = new List<string>();
              List<ValidationException> validationExceptions = new List<ValidationException>();
              foreach (ValidationResult validationResult in validationResults)
              {
                  validationExceptions.Add(new ValidationException(validationResult, null, null));
                  memberNames.AddRange(validationResult.MemberNames);
              }

              if (validationExceptions.Count == 1)  // eine Validationexception werfen
              {
                  throw validationExceptions.Single();
              }
              else  // AggregateException mit allen ValidationExceptions als InnerExceptions werfen
              {
                  throw new ValidationException($"Entity validation failed for {string.Join(", ", memberNames)}",
                      new AggregateException(validationExceptions));
              }
          }
      }
      return await _dbContext.SaveChangesAsync();
  }

  public async Task DeleteDatabaseAsync() => await _dbContext.Database.EnsureDeletedAsync();
  public async Task MigrateDatabaseAsync() => await _dbContext.Database.MigrateAsync();
  public async Task CreateDatabaseAsync() => await _dbContext.Database.EnsureCreatedAsync();

  public async ValueTask DisposeAsync()
  {
      await DisposeAsync(true);
      GC.SuppressFinalize(this);
  }

  protected virtual async ValueTask DisposeAsync(bool disposing)
  {
      if (!_disposed)
      {
          if (disposing)
          {
              await _dbContext.DisposeAsync();
          }
      }
      _disposed = true;
  }

  public void Dispose()
  {
      _dbContext?.Dispose();
  }
}
```

## Validation - ClassicMovieMaxDurationAttribute

```cs
/// <summary>
    /// Validiert ob Filme bis zu einem gewissen Produktionsjahr
    /// nicht länger als eine gewisse Anzahl an Minuten dauern.
    /// </summary>
    public class ClassicMovieMaxDurationAttribute : ValidationAttribute
    {
        public ClassicMovieMaxDurationAttribute(int isClassicMovieUntilYear, int maxDurationForClassicMovie)
        {
            IsClassicMovieUntilYear = isClassicMovieUntilYear;
            MaxDurationForClassicMovie = maxDurationForClassicMovie;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var movie = (Movie)validationContext.ObjectInstance;
            if (movie.Year <= IsClassicMovieUntilYear && movie.Duration > MaxDurationForClassicMovie)
            {
                return new ValidationResult($"Classical Movies (until year '{IsClassicMovieUntilYear}') may not last longer than {MaxDurationForClassicMovie} minutes!",
                    new List<string> { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }

        public int IsClassicMovieUntilYear { get; }
        public int MaxDurationForClassicMovie { get; }

    }
```


