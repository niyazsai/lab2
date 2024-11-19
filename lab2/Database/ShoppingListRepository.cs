using Microsoft.EntityFrameworkCore;

namespace lab2
{
    public class ShoppingListRepository
    {
        private readonly ShoppingListDbContext _context;

        public ShoppingListRepository(ShoppingListDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShoppingList>> GetAllShoppingListsAsync()
        {
            return await _context.ShoppingLists
                .Include(s => s.Products)
                .Include(s => s.History)
                    .ThenInclude(h => h.Entries)
                .ToListAsync();
        }

        public async Task AddShoppingListAsync(ShoppingList shoppingList)
        {
            await _context.ShoppingLists.AddAsync(shoppingList);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShoppingListAsync(ShoppingList shoppingList)
        {
            _context.ShoppingLists.Update(shoppingList);
            await _context.SaveChangesAsync();
        }
    }
}