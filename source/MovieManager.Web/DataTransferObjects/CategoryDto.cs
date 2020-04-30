using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Web.DataTransferObjects
{
  public class CategoryDto
  {
    [Required(ErrorMessage ="Der Kategoriename ist verpflichtend!")]
    public string CategoryName { get; set; }

    public CategoryDto() { }

    public CategoryDto(string categoryName)
    {
      CategoryName = categoryName;
    }
  }
}
