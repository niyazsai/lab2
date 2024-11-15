using System.ComponentModel.DataAnnotations;

namespace lab2;

public class HistoryEntry
{
    [Key]
    public int EntryId { get; set; } // Ключевой идентификатор

    [Required]
    public DateTime Timestamp { get; set; } // Время изменения

    [Required]
    public string Description { get; set; } // Описание изменения

    public int HistoryId { get; set; } // Связь с History
    public History History { get; set; } // Навигационное свойство
}