namespace lab2;

public class UserInterface
{
    private readonly CommandHandler commandHandler;
    private readonly ShoppingListRepository repository;

    public UserInterface(ShoppingListRepository repository, CommandHandler commandHandler)
    {
        this.repository = repository;
        this.commandHandler = commandHandler;
    }

    public void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Меню ===");
            Console.WriteLine("1. Создать новый список");
            Console.WriteLine("2. Просмотреть текущие списки");
            Console.WriteLine("3. Выйти");

            int choice = InputValidator.GetValidatedInt("Выберите действие: ", 1, 3);

            switch (choice)
            {
                case 1:
                    commandHandler.CreateNewList();
                    break;
                case 2:
                    commandHandler.ViewLists();
                    break;
                case 3:
                    repository.SaveData(commandHandler.GetShoppingLists()); // Используем SaveData напрямую
                    return;
            }
        }
    }
}