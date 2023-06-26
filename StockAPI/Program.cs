using StockAPI;
using Microsoft.Data.Sqlite;

string databaseFilePath = "test_stock_database.db";

SqliteConnection connection = new SqliteConnection($"Data Source={databaseFilePath}");

connection.Open();

string createPalletsTableQuery = "CREATE TABLE IF NOT EXISTS Pallets (ID INTEGER PRIMARY KEY AUTOINCREMENT, Width REAL, Height REAL, Depth REAL, Weight REAL)";
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

DatabaseReader reader = new DatabaseReader(databaseFilePath);

// Добавление тестовых значений

/*int palletId1 = reader.InsertPallet(width: 100, height: 100, depth: 100, weight: 500);

reader.InsertBox(palletId1, width: 50, height: 50, depth: 50, weight: 200, productionDate: new DateTime(2023, 1, 1));

reader.InsertBox(palletId1, width: 40, height: 40, depth: 40, weight: 150, productionDate: new DateTime(2023, 3, 15));

int palletId2 = reader.InsertPallet(width: 120, height: 80, depth: 50, weight: 300);

reader.InsertBox(palletId2, width: 100, height: 30, depth: 70, weight: 150, productionDate: new DateTime(2023, 1, 1));

reader.InsertBox(palletId2, width: 50, height: 70, depth: 80, weight: 200, productionDate: new DateTime(2023, 3, 15));

int palletId3 = reader.InsertPallet(width: 200, height: 50, depth: 200, weight: 700);

reader.InsertBox(palletId3, width: 80, height: 100, depth: 30, weight: 100, productionDate: new DateTime(2023, 1, 1));

reader.InsertBox(palletId3, width: 40, height: 30, depth: 20, weight: 50, productionDate: new DateTime(2023, 3, 15));*/

var sortedPallets = reader.ReadPalletsFromDatabase()
    .OrderBy(pallet => pallet.GetExpirationDate())
    .ThenBy(pallet => pallet.Weight);

Console.WriteLine("Паллеты, отсортированные по сроку годности и весу:");
foreach (var pallet in sortedPallets)
{
    Console.WriteLine(pallet);
}

var top3Pallets = sortedPallets
    .Take(3)
    .OrderBy(pallet => pallet.GetVolume());

Console.WriteLine("\nТоп 3 паллеты с наибольшим сроком годности, отсортированные по объему:");
foreach (var pallet in top3Pallets)
{
    Console.WriteLine(pallet);
}
