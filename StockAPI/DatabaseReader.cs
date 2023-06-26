using System;
using Microsoft.Data.Sqlite;

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
                            double weight = reader.GetDouble(4);

                            List<Box> boxes = GetBoxesForPallet(connection, palletId);

                            Pallet pallet = new Pallet
                            {
                                ID = palletId,
                                Width = width,
                                Height = height,
                                Depth = depth,
                                Weight = weight,
                                Boxes = boxes
                            };
                            pallets.Add(pallet);
                        }
                    }
                }
            }

            return pallets;
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

        public int InsertPallet(int width, int height, int depth, int weight)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source={databaseFilePath}"))
            {
                connection.Open();
                string query = "INSERT INTO Pallets (Width, Height, Depth, Weight) VALUES (@Width, @Height, @Depth, @Weight); SELECT last_insert_rowid();";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Width", width);
                    command.Parameters.AddWithValue("@Height", height);
                    command.Parameters.AddWithValue("@Depth", depth);
                    command.Parameters.AddWithValue("@Weight", weight);

                    int palletId = Convert.ToInt32(command.ExecuteScalar());
                    return palletId;
                }
            }
        }   

        public void InsertBox(int palletId, int width, int height, int depth, int weight, DateTime? productionDate = null)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source={databaseFilePath}"))
            {
                connection.Open();
                string query = "INSERT INTO Boxes (Width, Height, Depth, Weight, ProductionDate) VALUES (@Width, @Height, @Depth, @Weight, @ProductionDate); SELECT last_insert_rowid();";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Width", width);
                    command.Parameters.AddWithValue("@Height", height);
                    command.Parameters.AddWithValue("@Depth", depth);
                    command.Parameters.AddWithValue("@Weight", weight);
                    command.Parameters.AddWithValue("@ProductionDate", productionDate);

                    int boxId = Convert.ToInt32(command.ExecuteScalar());

                    string palletBoxesQuery = "INSERT INTO PalletBoxes (PalletId, BoxId) VALUES (@PalletId, @BoxId);";

                    using (var palletBoxesCommand = new SqliteCommand(palletBoxesQuery, connection))
                    {
                        palletBoxesCommand.Parameters.AddWithValue("@PalletId", palletId);
                        palletBoxesCommand.Parameters.AddWithValue("@BoxId", boxId);

                        palletBoxesCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }

}

