using Microsoft.EntityFrameworkCore;

namespace lab2
{
    public class ShoppingListDbContext : DbContext
    {
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<History> Histories { get; set; }

        public ShoppingListDbContext(DbContextOptions<ShoppingListDbContext> options)
            : base(options)
        {
            // Обеспечиваем создание базы данных при первом обращении
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=shoppinglist.db");
        }
        
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     // Настраиваем отношения между сущностями
        //     modelBuilder.Entity<ShoppingList>()
        //         .HasMany(s => s.Products)
        //         .WithOne(p => p.ShoppingList)
        //         .HasForeignKey(p => p.ShoppingListId)
        //         .OnDelete(DeleteBehavior.Cascade);
        // }
    }
}
