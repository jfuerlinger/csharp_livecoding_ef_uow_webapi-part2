using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieManager.Persistence;

namespace MovieManager.Test
{
  [TestClass]
  public class CategoryRepositoryTests
  {
    [TestMethod]
    public void GetCategoryWithMostMovies_CallMethod_ShouldReturnActionWith46Movies()
    {
      // Arrange
      using UnitOfWork uow = new UnitOfWork();

      // Act
      var categoryWithMostMoviews = uow.CategoryRepository.GetCategoryWithMostMovies();

      // Assert
      Assert.IsNotNull(categoryWithMostMoviews);
      Assert.AreEqual("Action", categoryWithMostMoviews.Category.CategoryName);
      Assert.AreEqual(46, categoryWithMostMoviews.NumberOfMovies);
    }

    [TestMethod]
    public void GetCategoryStatistics_CallMethod_ShouldReturn11Categories()
    {
      // Arrange
      using UnitOfWork uow = new UnitOfWork();

      // Act
      var categoryStatistics = uow.CategoryRepository.GetCategoryStatistics();

      // Assert
      Assert.IsNotNull(categoryStatistics);
      Assert.AreEqual(11, categoryStatistics.Length);
    }

    [TestMethod]
    public void GetCategoriesWithAverageLengthOfMovies_CallMethod_ShouldReturn11Categories()
    {
      // Arrange
      using UnitOfWork uow = new UnitOfWork();

      // Act
      var categories = uow.CategoryRepository.GetCategoriesWithAverageLengthOfMovies();

      // Assert
      Assert.IsNotNull(categories);
      Assert.AreEqual(11, categories.Length);
    }

    [TestMethod]
    public void GetYearWithMostPublicationsForCategory_CallMethod_ShouldReturn11Categories()
    {
      // Arrange
      using UnitOfWork uow = new UnitOfWork();

      // Act
      var yearWithMostActionMovies = uow.CategoryRepository.GetYearWithMostPublicationsForCategory("Action");

      // Assert
      Assert.AreEqual(2008, yearWithMostActionMovies);
    }
  }
}
