using System;
using NUnit.Framework;

namespace StockAPI.UnitTests
{
    [TestFixture]
	public class PalletTests
	{
        [Test]
        public void GetVolume_ReturnsCorrectVolume()
        {
            // Arrange
            Pallet pallet = new Pallet
            {
                ID = 1,
                Width = 100,
                Height = 200,
                Depth = 300,
                Weight = 1,
            };
            Box box1 = new Box
            {
                ID = 1,
                Width = 10,
                Height = 20,
                Depth = 30,
                Weight = 1,
                ProductionDate = new DateTime(2023, 1, 1)
            };
            Box box2 = new Box
            {
                ID = 2,
                Width = 10,
                Height = 20,
                Depth = 30,
                Weight = 1,
                ProductionDate = new DateTime(2023, 1, 1)
            };
            pallet.Boxes.Add(box1);
            pallet.Boxes.Add(box2);

            // Act
            double volume = pallet.GetVolume();

            // Assert
            Assert.AreEqual(66000, volume);
        }
    }
}

