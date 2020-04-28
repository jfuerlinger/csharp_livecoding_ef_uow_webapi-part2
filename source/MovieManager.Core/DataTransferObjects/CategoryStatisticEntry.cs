using MovieManager.Core.Entities;

namespace MovieManager.Core.DTOs
{
    public class CategoryStatisticEntry
    {
        public Category Category { get; set; }
        public int NumberOfMovies { get; set; }
        public int TotalDuration { get; set; }
    }
}
