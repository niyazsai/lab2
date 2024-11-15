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
        public List<Product> Products { get; set; } = new List<Product>();
        public History history { get; set; } = new History();

        public ShoppingList() { } // Конструктор по умолчанию для EF Core

        public ShoppingList(string name)
        {
            Name = name;
        }

        public void AddProduct(Product product)
        {
            product.ShoppingList = this;
            Products.Add(product);
            history.AddEntry($"Добавлен товар '{product.Name}'");
        }

        public void RemoveProduct(int index)
        {
            if (index >= 0 && index < Products.Count)
            {
                var product = Products.ElementAt(index);
                Products.Remove(product);
                history.AddEntry($"Удален товар '{product.Name}'");
            }
        }
        
        public void MarkAsPurchased(int index)
        {
            if (index >= 0 && index < Products.Count)
            {
                Products[index].IsPurchased = true;
                Products[index].PurchaseDate = DateTime.Now;
                history.AddEntry($"Товар '{Products[index].Name}' отмечен как купленный");
            }
        }
    }
}