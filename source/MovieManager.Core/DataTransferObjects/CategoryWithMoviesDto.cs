using MovieManager.Core.Entities;
using System.Linq;

namespace MovieManager.Core.DataTransferObjects
{
    public class CategoryWithMoviesDto
    {
        public string CategoryName { get; set; }
        public MovieDto[] Movies { get; set; }

        public CategoryWithMoviesDto(Category category)
        {
            CategoryName = category.CategoryName;
            Movies = category
                .Movies
                .Select(movie => new MovieDto()
                {
                    Duration = movie.Duration,
                    Title = movie.Title,
                    Year = movie.Year
                }).ToArray();
        }
    }


}
