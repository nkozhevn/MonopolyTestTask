using Microsoft.Data.Sqlite;

namespace StockAPI
{
    public class DatabaseWriter
    {
        private string databaseFilePath;
        private DatabaseReader reader;

        public DatabaseWriter(string databaseFilePath)
        {
            this.databaseFilePath = databaseFilePath;
            this.reader = new DatabaseReader(databaseFilePath);
        }

        public void CreateDatabase()
        {
            SqliteConnection connection = new SqliteConnection($"Data Source={databaseFilePath}");

            connection.Open();

            string createPalletsTableQuery = "CREATE TABLE IF NOT EXISTS Pallets (ID INTEGER PRIMARY KEY AUTOINCREMENT, Width REAL, Height REAL, Depth REAL)";
            using (SqliteCommand command = new SqliteCommand(createPalletsTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string createBoxesTableQuery = "CREATE TABLE IF NOT EXISTS Boxes (ID INTEGER PRIMARY KEY AUTOINCREMENT, Width REAL, Height REAL, Depth REAL, Weight REAL, ProductionDate DATE)";
            using (SqliteCommand command = new SqliteCommand(createBoxesTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string createPalletBoxesTableQuery = "CREATE TABLE IF NOT EXISTS PalletBoxes (PalletID INTEGER, BoxID INTEGER, FOREIGN KEY (PalletID) REFERENCES Pallets (ID), FOREIGN KEY (BoxID) REFERENCES Boxes (ID))";
            using (SqliteCommand command = new SqliteCommand(createPalletBoxesTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        public int InsertPallet(double width, double height, double depth)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source={databaseFilePath}"))
            {
                connection.Open();
                string query = "INSERT INTO Pallets (Width, Height, Depth) VALUES (@Width, @Height, @Depth); SELECT last_insert_rowid();";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Width", width);
                    command.Parameters.AddWithValue("@Height", height);
                    command.Parameters.AddWithValue("@Depth", depth);

                    int palletId = Convert.ToInt32(command.ExecuteScalar());
                    return palletId;
                }
            }
        }

        public int InsertBox(int palletId, double width, double height, double depth, double weight, DateTime? productionDate = null)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source={databaseFilePath}"))
            {
                connection.Open();

                Pallet pallet = reader.ReadPallet(connection, palletId);
                if(width > pallet.Width || height > pallet.Height || depth > pallet.Depth)
                {
                    throw new Exception("Размеры коробки превышают размеры палеты.");
                }

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

                    return boxId;
                }
            }
        }
    }

}

