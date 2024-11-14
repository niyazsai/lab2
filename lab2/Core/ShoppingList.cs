using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab2
{
    public class ShoppingList
    {
        [Key]
        public int ShoppingListId { get; set; } // Ключевой идентификатор

        [Required]
        public string Name { get; set; }

        // Навигационное свойство
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public ShoppingList() { } // Конструктор по умолчанию для EF Core

        public ShoppingList(string name)
        {
            Name = name;
        }

        public void AddProduct(Product product)
        {
            product.ShoppingList = this;
            Products.Add(product);
        }

        public void RemoveProduct(int index)
        {
            if (index >= 0 && index < Products.Count)
            {
                var product = Products.ElementAt(index);
                Products.Remove(product);
            }
        }
    }
}