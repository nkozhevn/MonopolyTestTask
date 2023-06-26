using NUnit.Framework;

namespace StockAPI.UnitTests
{
    [TestFixture]
    public class DatabaseReaderTests
    {
        [Test]
        public void ReadBoxesFromDatabase_ReturnsCorrectNumberOfBoxes()
        {
            // Arrange
            string databaseFilePath = "test_stock_database.db";
            DatabaseReader databaseReader = new DatabaseReader(databaseFilePath);

            // Act
            List<Box> boxes = databaseReader.ReadBoxesFromDatabase();

            // Assert
            Assert.AreEqual(2, boxes.Count);
        }
    }
}

