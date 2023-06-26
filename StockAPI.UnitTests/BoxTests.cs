using NUnit.Framework;

namespace StockAPI.UnitTests
{
    [TestFixture]
	public class BoxTests
	{
        [Test]
        public void GetVolume_ReturnsCorrectVolume()
        {
            // Arrange
            Box box = new Box
            {
                ID = 1,
                Width = 10,
                Height = 20,
                Depth = 30,
                Weight = 1,
                ProductionDate = new DateTime(2023, 1, 1)
            };

            // Act
            double volume = box.GetVolume();

            // Assert
            Assert.AreEqual(6000, volume);
        }
    }
}

