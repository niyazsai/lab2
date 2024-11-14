using System.Collections.Generic;
using System.Threading.Tasks;

namespace lab2
{
    public class CommandHandler
    {
        private readonly ShoppingListRepository _repository;

        public CommandHandler(ShoppingListRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateNewListAsync()
        {
            Console.Clear();
            string name = InputValidator.GetNonEmptyString("Введите название нового списка: ");
            var shoppingList = new ShoppingList(name);
            await _repository.AddShoppingListAsync(shoppingList);
            Console.WriteLine("Новый список создан. Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
        }

        public async Task ViewListsAsync()
        {
            Console.Clear();
            var shoppingLists = await _repository.GetAllShoppingListsAsync();
            if (shoppingLists.Count == 0)
            {
                Console.WriteLine("Нет доступных списков.");
                Console.WriteLine("Нажмите любую клавишу для продолжения.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Доступные списки:");
            for (int i = 0; i < shoppingLists.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {shoppingLists[i].Name}");
            }

            int choice = InputValidator.GetValidatedInt("Выберите список для управления или 0 для возврата: ", 0, shoppingLists.Count);
            if (choice == 0)
            {
                return;
            }

            var selectedList = shoppingLists[choice - 1];
            await ManageListAsync(selectedList);
        }

        private async Task ManageListAsync(ShoppingList shoppingList)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"== {shoppingList.Name} ==");
                Console.WriteLine("1. Добавить товары");
                Console.WriteLine("2. Просмотреть товары");
                Console.WriteLine("3. Удалить товар");
                Console.WriteLine("4. Удалить список");
                Console.WriteLine("5. Назад");

                int choice = InputValidator.GetValidatedInt("Выберите действие: ", 1, 5);

                switch (choice)
                {
                    case 1:
                        await EditListAsync(shoppingList);
                        break;
                    case 2:
                        ViewProducts(shoppingList);
                        break;
                    case 3:
                        await RemoveProductAsync(shoppingList);
                        break;
                    case 4:
                        await _repository.DeleteShoppingListAsync(shoppingList.ShoppingListId);
                        Console.WriteLine("Список удален. Нажмите любую клавишу для продолжения.");
                        Console.ReadLine();
                        return;
                    case 5:
                        return;
                }
            }
        }

        private async Task EditListAsync(ShoppingList shoppingList)
        {
            Console.Clear();
            Console.WriteLine("Добавьте товары в список. Введите 'готово' для завершения.");
            while (true)
            {
                string productName = InputValidator.GetNonEmptyString("Товар: ");
                if (productName.ToLower() == "готово")
                {
                    break;
                }

                Console.Write("Категория товара: ");
                string category = Console.ReadLine();

                var product = new Product(productName, category)
                {
                    ShoppingListId = shoppingList.ShoppingListId
                };

                shoppingList.AddProduct(product);
            }
            await _repository.UpdateShoppingListAsync(shoppingList);
            Console.WriteLine("Список успешно обновлен. Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
        }

        private void ViewProducts(ShoppingList shoppingList)
        {
            Console.Clear();
            if (shoppingList.Products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
            }
            else
            {
                Console.WriteLine("Товары в списке:");
                foreach (var product in shoppingList.Products)
                {
                    Console.WriteLine($"- {product.Name} [{product.Category}]");
                }
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
        }

        private async Task RemoveProductAsync(ShoppingList shoppingList)
        {
            Console.Clear();
            if (shoppingList.Products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
                Console.WriteLine("Нажмите любую клавишу для продолжения.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Товары в списке:");
            var products = shoppingList.Products.ToList();
            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];

                Console.WriteLine($"{i + 1}. {product.Name} [{product.Category}]");
            }

            int choice = InputValidator.GetValidatedInt("Выберите номер товара для удаления: ", 1, products.Count);
            var productToRemove = products[choice - 1];
            shoppingList.Products.Remove(productToRemove);
            await _repository.UpdateShoppingListAsync(shoppingList);

            Console.WriteLine("Товар успешно удален. Нажмите любую клавишу для продолжения.");
            Console.ReadLine();
        }
    }
}
