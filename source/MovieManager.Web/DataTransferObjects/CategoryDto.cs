using System.ComponentModel.DataAnnotations;

namespace MovieManager.Web.DataTransferObjects
{
  public class CategoryDto
  {
    [Required]
    [MinLength(2, ErrorMessage = "Der Name muss aus mind. 2 Zeichen bestehen")]
    public string CategoryName { get; set; }

    public CategoryDto()
    {
    }

    public CategoryDto(string categoryName)
    {
      CategoryName = categoryName;
    }
  }
}
