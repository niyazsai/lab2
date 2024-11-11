namespace lab2;

class Program
{
    static async Task Main(string[] args)
    {
        var connectionString = "Data Source=shoppinglists.db"; // Пусть базы данных
        var dbContext = new ShoppingListDbContext(connectionString);
        await dbContext.InitializeAsync();

        var repository = new ShoppingListRepository(dbContext);
        var commandHandler = await CommandHandler.Initialize(repository);
        var ui = new UserInterface(repository, commandHandler);
        ui.MainMenu();
    }
}
