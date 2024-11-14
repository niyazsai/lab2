using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab2
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; } // Ключевой идентификатор

        [Required]
        public string Name { get; set; }

        public string Category { get; set; }

        // Ссылка на список покупок
        public int ShoppingListId { get; set; }
        public ShoppingList ShoppingList { get; set; }

        // Конструктор по умолчанию для EF Core
        public Product() { }

        public Product(string name, string category)
        {
            Name = name;
            Category = category;
        }
    }
}