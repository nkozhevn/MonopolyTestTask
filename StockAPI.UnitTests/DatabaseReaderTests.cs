using NUnit.Framework;

namespace StockAPI.UnitTests
{
    [TestFixture]
    public class DatabaseReaderTests
    {
        private const string DatabaseFilePath = "test_stock_database.db";
        private DatabaseWriter writer = new DatabaseWriter(DatabaseFilePath);
        private DatabaseReader reader = new DatabaseReader(DatabaseFilePath);

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
        public void ReadPalletsFromDatabase_WithEmptyDatabase_ReturnsEmptyList()
        {
            // Act
            List<Pallet> pallets = reader.ReadPalletsFromDatabase();

            // Assert
            Assert.That(pallets, Is.Empty);
        }

        [Test]
        public void ReadBoxesFromDatabase_WithEmptyDatabase_ReturnsEmptyList()
        {
            // Act
            List<Box> boxes = reader.ReadBoxesFromDatabase();

            // Assert
            Assert.That(boxes, Is.Empty);
        }

        [Test]
        public void ReadPalletsFromDatabase_WithDataInDatabase_ReturnsCorrectPallets()
        {
            // Arrange
            double width = 100;
            double height = 100;
            double depth = 100;
            int expectedPalletCount = 1;

            int palletId = writer.InsertPallet(width, height, depth);

            // Act
            List<Pallet> pallets = reader.ReadPalletsFromDatabase();

            // Assert
            Assert.That(pallets, Is.Not.Empty);
            Assert.That(pallets.Count, Is.EqualTo(expectedPalletCount));

            Pallet pallet = pallets[0];
            Assert.That(pallet.ID, Is.EqualTo(palletId));
            Assert.That(pallet.Width, Is.EqualTo(width));
            Assert.That(pallet.Height, Is.EqualTo(height));
            Assert.That(pallet.Depth, Is.EqualTo(depth));
        }

        [Test]
        public void ReadBoxesFromDatabase_WithDataInDatabase_ReturnsCorrectBoxes()
        {
            // Arrange
            int palletId = 1;
            double width = 50;
            double height = 50;
            double depth = 50;
            double weight = 200;
            DateTime productionDate = new DateTime(2023, 1, 1);
            int expectedBoxCount = 1;

            int boxId = writer.InsertBox(palletId, width, height, depth, weight, productionDate);

            // Act
            List<Box> boxes = reader.ReadBoxesFromDatabase();

            // Assert
            Assert.That(boxes, Is.Not.Empty);
            Assert.That(boxes.Count, Is.EqualTo(expectedBoxCount));

            Box box = boxes[0];
            Assert.That(box.ID, Is.EqualTo(boxId));
            Assert.That(box.Width, Is.EqualTo(width));
            Assert.That(box.Height, Is.EqualTo(height));
            Assert.That(box.Depth, Is.EqualTo(depth));
            Assert.That(box.Weight, Is.EqualTo(weight));
            Assert.That(box.ProductionDate, Is.EqualTo(productionDate));
        }
    }
}
