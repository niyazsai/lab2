using System.ComponentModel.DataAnnotations;

namespace lab2;

public class History
{
    [Key]
    public int HistoryId { get; set; } // Ключевой идентификатор

    [Required]
    public int ShoppingListId { get; set; } // Связь с ShoppingList

    // Навигационное свойство для списка изменений
    public List<HistoryEntry> Entries { get; set; } = new List<HistoryEntry>();

    // Добавление новой записи
    public void AddEntry(string description)
    {
        Entries.Add(new HistoryEntry
        {
            Timestamp = DateTime.Now,
            Description = description
        });
    }
}