using NUnit.Framework;

namespace StockAPI.UnitTests
{
    [TestFixture]
    public class DatabaseWriterTests
    {
        private const string DatabaseFilePath = "test_stock_database.db";
        private DatabaseWriter writer = new DatabaseWriter(DatabaseFilePath);

        [SetUp]
        public void Setup()
        {
            writer.CreateDatabase();
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(DatabaseFilePath))
            {
                File.Delete(DatabaseFilePath);
            }
        }

        [Test]
        public void InsertPallet_WithValidData_ReturnsValidPalletId()
        {
            // Arrange
            double width = 100;
            double height = 100;
            double depth = 100;

            // Act
            int palletId = writer.InsertPallet(width, height, depth);

            // Assert
            Assert.That(palletId, Is.GreaterThan(0));
        }

        [Test]
        public void InsertBox_WithValidData_ReturnsValidBoxId()
        {
            // Arrange
            int palletId = 1;
            double width = 50;
            double height = 50;
            double depth = 50;
            double weight = 200;
            DateTime productionDate = new DateTime(2023, 1, 1);

            // Act
            int boxId = writer.InsertBox(palletId, width, height, depth, weight, productionDate);

            // Assert
            Assert.That(boxId, Is.GreaterThan(0));
        }
    }
}

