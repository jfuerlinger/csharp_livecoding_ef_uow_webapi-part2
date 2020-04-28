using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieManager.Core.Validation
{
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
}
