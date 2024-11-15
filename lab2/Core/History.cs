using System.ComponentModel.DataAnnotations;

namespace lab2;

public class History
{
    [Key]
    public int HistoryId { get; set; } // Ключевой идентификатор
    
    [Required]
    public List<string> Changes { get; set; } = new List<string>();
    //public List<HistoryEntry> Entries { get; private set; } = new List<HistoryEntry>();


    public void AddEntry(string entry)
    {
        Changes.Add($"{DateTime.Now}: {entry}");
        //Entries.Add(new HistoryEntry
        // {
        //     Description = entry,
        //     Timestamp = DateTime.Now
        // });
    }
}