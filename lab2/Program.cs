using Microsoft.EntityFrameworkCore;

namespace lab2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingListDbContext>();
            optionsBuilder.UseSqlite("Data Source=shoppinglist.db");
            
            using (var context = new ShoppingListDbContext(optionsBuilder.Options))
            {
                var repository = new ShoppingListRepository(context);
                var commandHandler = new CommandHandler(repository);
                var ui = new UserInterface(commandHandler);
                await ui.MainMenuAsync();
            }
        }
    }
}