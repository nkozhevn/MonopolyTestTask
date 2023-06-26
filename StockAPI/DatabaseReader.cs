using System;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace StockAPI
{
    public class DatabaseReader
    {
        private string databaseFilePath;

        public DatabaseReader(string databaseFilePath)
        {
            this.databaseFilePath = databaseFilePath;
        }

        public List<Pallet> ReadPalletsFromDatabase()
        {
            List<Pallet> pallets = new List<Pallet>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={databaseFilePath}"))
            {
                connection.Open();

                string selectPalletsQuery = "SELECT * FROM Pallets";
                using (SqliteCommand command = new SqliteCommand(selectPalletsQuery, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int palletId = reader.GetInt32(0);
                            double width = reader.GetDouble(1);
                            double height = reader.GetDouble(2);
                            double depth = reader.GetDouble(3);

                            List<Box> boxes = GetBoxesForPallet(connection, palletId);

                            Pallet pallet = new Pallet
                            {
                                ID = palletId,
                                Width = width,
                                Height = height,
                                Depth = depth,
                                Boxes = boxes
                            };
                            pallets.Add(pallet);
                        }
                    }
                }
            }

            return pallets;
        }

        public Pallet ReadPallet(SqliteConnection connection, int palletId)
        {
            string query = "SELECT * FROM Pallets WHERE ID = @PalletId;";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PalletId", palletId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        double width = reader.GetDouble(1);
                        double height = reader.GetDouble(2);
                        double depth = reader.GetDouble(3);

                        List<Box> boxes = GetBoxesForPallet(connection, palletId);

                        return new Pallet
                        {
                            ID = palletId,
                            Width = width,
                            Height = height,
                            Depth = depth,
                            Boxes = boxes
                        };
                    }
                    else
                    {
                        throw new Exception("Палета с указанным идентификатором не найдена.");
                    }
                }
            }
        }

        private List<Box> GetBoxesForPallet(SqliteConnection connection, int palletId)
        {
            List<Box> boxes = new List<Box>();

            string selectBoxesQuery = "SELECT b.* FROM Boxes b INNER JOIN PalletBoxes pb ON b.ID = pb.BoxID WHERE pb.PalletID = @palletId";
            using (SqliteCommand command = new SqliteCommand(selectBoxesQuery, connection))
            {
                command.Parameters.AddWithValue("@palletId", palletId);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int boxId = reader.GetInt32(0);
                        double width = reader.GetDouble(1);
                        double height = reader.GetDouble(2);
                        double depth = reader.GetDouble(3);
                        double weight = reader.GetDouble(4);
                        DateTime productionDate = reader.GetDateTime(5);

                        Box box = new Box
                        {
                            ID = boxId,
                            Width = width,
                            Height = height,
                            Depth = depth,
                            Weight = weight,
                            ProductionDate = productionDate
                        };
                        boxes.Add(box);
                    }
                }
            }

            return boxes;
        }

        public List<Box> ReadBoxesFromDatabase()
        {
            List<Box> boxes = new List<Box>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={databaseFilePath}"))
            {
                connection.Open();

                string selectBoxesQuery = "SELECT * FROM Boxes";
                using (SqliteCommand command = new SqliteCommand(selectBoxesQuery, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int boxId = reader.GetInt32(0);
                            double width = reader.GetDouble(1);
                            double height = reader.GetDouble(2);
                            double depth = reader.GetDouble(3);
                            double weight = reader.GetDouble(4);
                            DateTime productionDate = reader.GetDateTime(5);

                            Box box = new Box
                            {
                                ID = boxId,
                                Width = width,
                                Height = height,
                                Depth = depth,
                                Weight = weight,
                                ProductionDate = productionDate
                            };

                            boxes.Add(box);
                        }
                    }
                }
            }

            return boxes;
        }
    }
}
