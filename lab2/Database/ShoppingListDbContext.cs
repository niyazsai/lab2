using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab2
{
    public class ShoppingListDbContext
    {
        private readonly string connectionString;

        public ShoppingListDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task InitializeAsync()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();

                var createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ShoppingLists (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Products (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Category TEXT NOT NULL,
                        IsPurchased INTEGER NOT NULL,
                        PurchaseDate TEXT,
                        ShoppingListId INTEGER,
                        FOREIGN KEY (ShoppingListId) REFERENCES ShoppingLists(Id)
                    );";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<ShoppingList>> GetShoppingListsAsync()
        {
            var shoppingLists = new List<ShoppingList>();

            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT Id, Name FROM ShoppingLists";
                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var list = new ShoppingList(reader.GetString(1));
                            list.Id = reader.GetInt32(0);
                            shoppingLists.Add(list);
                        }
                    }
                }

                foreach (var shoppingList in shoppingLists)
                {
                    shoppingList.Products = await GetProductsAsync(shoppingList.Id);
                }
            }

            return shoppingLists;
        }

        public async Task<List<Product>> GetProductsAsync(int shoppingListId)
        {
            var products = new List<Product>();

            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT Id, Name, Category, IsPurchased, PurchaseDate FROM Products WHERE ShoppingListId = @ShoppingListId";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ShoppingListId", shoppingListId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var product = new Product(reader.GetString(1), reader.GetString(2))
                            {
                                Id = reader.GetInt32(0),
                                IsPurchased = reader.GetBoolean(3),
                                PurchaseDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                            };
                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }

        public async Task SaveShoppingListAsync(ShoppingList shoppingList)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();

                var insertShoppingListQuery = "INSERT INTO ShoppingLists (Name) VALUES (@Name)";
                using (var command = new SqliteCommand(insertShoppingListQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", shoppingList.Name);
                    await command.ExecuteNonQueryAsync();

                    // Используем запрос для получения последнего вставленного Id
                    command.CommandText = "SELECT last_insert_rowid()";
                    shoppingList.Id = (int)(long)await command.ExecuteScalarAsync();
                }

                foreach (var product in shoppingList.Products)
                {
                    var insertProductQuery = @"
                        INSERT INTO Products (Name, Category, IsPurchased, PurchaseDate, ShoppingListId) 
                        VALUES (@Name, @Category, @IsPurchased, @PurchaseDate, @ShoppingListId)";

                    using (var command = new SqliteCommand(insertProductQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@Category", product.Category);
                        command.Parameters.AddWithValue("@IsPurchased", product.IsPurchased ? 1 : 0);
                        command.Parameters.AddWithValue("@PurchaseDate", product.PurchaseDate?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ShoppingListId", shoppingList.Id);

                        await command.ExecuteNonQueryAsync();

                        // Получаем последний вставленный Id для продукта
                        command.CommandText = "SELECT last_insert_rowid()";
                        product.Id = (int)(long)await command.ExecuteScalarAsync();
                    }
                }
            }
        }
    }
}
