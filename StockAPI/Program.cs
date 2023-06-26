using StockAPI;
using Microsoft.Data.Sqlite;

string databaseFilePath = "test_stock_database.db";

DatabaseWriter writer = new DatabaseWriter(databaseFilePath);
writer.CreateDatabase();

// Добавление тестовых значений

/*int palletId1 = writer.InsertPallet(width: 100, height: 100, depth: 100);

writer.InsertBox(palletId1, width: 50, height: 50, depth: 50, weight: 200, productionDate: new DateTime(2023, 1, 1));

writer.InsertBox(palletId1, width: 40, height: 40, depth: 40, weight: 150, productionDate: new DateTime(2023, 3, 15));

int palletId2 = writer.InsertPallet(width: 120, height: 80, depth: 100);

writer.InsertBox(palletId2, width: 100, height: 30, depth: 70, weight: 150, productionDate: new DateTime(2023, 1, 1));

writer.InsertBox(palletId2, width: 50, height: 70, depth: 80, weight: 200, productionDate: new DateTime(2023, 3, 15));

int palletId3 = writer.InsertPallet(width: 200, height: 50, depth: 200);

writer.InsertBox(palletId3, width: 80, height: 20, depth: 30, weight: 100, productionDate: new DateTime(2023, 1, 1));

writer.InsertBox(palletId3, width: 40, height: 30, depth: 20, weight: 50, productionDate: new DateTime(2023, 3, 15));*/

DatabaseReader reader = new DatabaseReader(databaseFilePath);

var sortedPallets = reader.ReadPalletsFromDatabase()
    .OrderBy(pallet => pallet.GetExpirationDate())
    .ThenBy(pallet => pallet.GetWeight());

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
