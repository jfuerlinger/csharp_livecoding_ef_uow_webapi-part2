using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieManager.Persistence;

namespace MovieManager.Test
{
  [TestClass]
  public class MovieRepositoryTests
  {

    [TestMethod]
    public void GetLongestMovie_CallMethod_ShouldReturnBarberShop()
    {
      // Arrange
      using UnitOfWork uow = new UnitOfWork();

      // Act
      var longestMovie = uow.MovieRepository.GetLongestMovie();

      // Assert
      Assert.AreEqual("Barbershop", longestMovie.Title);
    }

    [TestMethod]
    public void GetCount_CallMethod_ShouldReturn148()
    {
      // Arrange
      using UnitOfWork uow = new UnitOfWork();

      // Act
      var cntOfMovies = uow.MovieRepository.GetCount();

      // Assert
      Assert.AreEqual(148, cntOfMovies);
    }
  }
}
