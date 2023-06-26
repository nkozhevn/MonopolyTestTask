using System;

namespace StockAPI
{
	public class Pallet
	{
        public int ID { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public double Weight { get; set; }
        public List<Box> Boxes { get; set; } = new List<Box>();

        public DateTime GetExpirationDate()
        {
            DateTime expirationDate = DateTime.MinValue;

            foreach (var box in Boxes)
            {
                DateTime boxExpirationDate = box.GetExpirationDate();
                if (expirationDate == DateTime.MinValue || boxExpirationDate < expirationDate)
                {
                    expirationDate = boxExpirationDate;
                }
            }

            return expirationDate;
        }

        public double GetVolume()
        {
            double boxesVolume = Boxes.Sum(box => box.GetVolume());
            double palletVolume = Width * Height * Depth;
            return boxesVolume + palletVolume;
        }

        public override string ToString()
        {
            return "Палета №" + ID + ": \n" +
                "Ширина: " + Width + "\n" +
                "Высота: " + Height + "\n" +
                "Глубина: " + Depth + "\n" +
                "Вес: " + Weight + "\n" +
                "Годна до: " + GetExpirationDate() + "\n" +
                "Объем: " + GetVolume() + "\n";
        }
    }
}

