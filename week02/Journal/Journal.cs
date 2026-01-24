using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Journal;

public class Journal
{
    private List<Entry> _entries = new List<Entry>();

    public void AddEntry(Entry entry)
    {
        _entries.Add(entry);
    }

    public void DisplayEntries()
    {
        if (_entries.Count == 0)
        {
            Console.WriteLine("Journal is empty.");
            return;
        }

        foreach (Entry entry in _entries)
        {
            entry.Display();
        }
    }

    public void SaveToFile(string filename)
    {
        string json = JsonSerializer.Serialize(
            _entries,
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(filename, json);
        Console.WriteLine("Journal saved successfully.");
    }

    public void LoadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.");
            return;
        }

        string json = File.ReadAllText(filename);
        _entries = JsonSerializer.Deserialize<List<Entry>>(json)
                   ?? new List<Entry>();

        Console.WriteLine("Journal loaded successfully.");
    }
}
