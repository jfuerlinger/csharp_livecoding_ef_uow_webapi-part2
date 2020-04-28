using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManager.Core.Entities
{
  public class Movie : EntityObject
  {
    [Required]
    public string Title { get; set; }

    [Required]
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int Duration { get; set; } //in Minuten

    [Required]
    public int Year { get; set; }
  }
}
