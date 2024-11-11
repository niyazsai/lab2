namespace lab2
{
    public class ShoppingListRepository
    {
        private readonly ShoppingListDbContext dbContext;

        public ShoppingListRepository(ShoppingListDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<ShoppingList>> LoadData()
        {
            return await dbContext.GetShoppingListsAsync();
        }

        public async Task SaveData(List<ShoppingList> shoppingLists)
        {
            foreach (var shoppingList in shoppingLists)
            {
                await dbContext.SaveShoppingListAsync(shoppingList);
            }
        }
    }
}