using System;

namespace StockAPI
{
	public class Box
	{
        public int ID { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public double Weight { get; set; }
        public DateTime ProductionDate { get; set; }

        public DateTime GetExpirationDate()
        {
            if (ProductionDate != default)
            {
                return ProductionDate.AddDays(100);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public double GetVolume()
        {
            return Width * Height * Depth;
        }

        public override string ToString()
        {
            return "Палета №" + ID + ": \n" +
                "Ширина: " + Width + "\n" +
                "Высота: " + Height + "\n" +
                "Глубина: " + Depth + "\n" +
                "Вес: " + Weight + "\n" +
                "Дата изготовления: " + ProductionDate + "\n" +
                "Годна до: " + GetExpirationDate() + "\n" +
                "Объем: " + GetVolume() + "\n";
        }
    }
}

