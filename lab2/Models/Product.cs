using System.ComponentModel.DataAnnotations;

namespace lab2
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Category { get; set; }
        public bool IsPurchased { get; set; }
        public DateTime PurchaseDate { get; set; }
        
        public int ShoppingListId { get; set; }
        public ShoppingList ShoppingList { get; set; }
        
        public Product() { }

        public Product(string name, string category)
        {
            Name = name;
            Category = category;
            IsPurchased = false;
        }
    }
}