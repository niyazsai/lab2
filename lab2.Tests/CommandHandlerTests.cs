using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace lab2.Tests
{
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [TestFixture]
    public class CommandHandlerTests
    {
        [Test]
        public async Task GetShoppingLists_ReturnsShoppingLists()
        {
            // Создаем новое соединение и контекст для каждого теста
            using var sqliteConnection = new SqliteConnection("DataSource=:memory:");
            sqliteConnection.Open();

            var options = new DbContextOptionsBuilder<ShoppingListDbContext>()
                .UseSqlite(sqliteConnection)
                .Options;

            using var dbContext = new ShoppingListDbContext(options);
            dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            var repository = new ShoppingListRepository(dbContext);
            var commandHandler = new CommandHandler(repository);

            var testList = new ShoppingList("Test List");
            await repository.AddShoppingListAsync(testList);

            var shoppingLists = await repository.GetAllShoppingListsAsync();

            Assert.IsNotNull(shoppingLists);
            Assert.AreEqual(1, shoppingLists.Count);
            Assert.AreEqual("Test List", shoppingLists.First().Name);
            string resetSql = @"
            DELETE FROM sqlite_sequence WHERE name = 'tests';
        ";
        }

        [Test]
        public async Task ViewLists_WithLists_DisplaysLists()
        {
            using var sqliteConnection = new SqliteConnection("DataSource=:memory:");
            sqliteConnection.Open();

            var options = new DbContextOptionsBuilder<ShoppingListDbContext>()
                .UseSqlite(sqliteConnection)
                .Options;

            using var dbContext = new ShoppingListDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();

            var repository = new ShoppingListRepository(dbContext);
            var commandHandler = new CommandHandler(repository);

            var testList = new ShoppingList("Test List");
            await repository.AddShoppingListAsync(testList);

            var input = new StringReader("0\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            await commandHandler.ViewListsAsync();

            StringAssert.Contains("Доступные списки:", output.ToString());
            StringAssert.Contains("1. Test List", output.ToString());
        }
    }
}
