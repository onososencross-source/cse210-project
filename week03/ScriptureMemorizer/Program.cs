// W03 Project: Scripture Memorizer Program
// Exceeding Requirements:
// - Supports a small library of scriptures and randomly selects one at runtime.
// - Ensures only non-hidden words are selected when hiding words.
// - Hides a configurable number of words per round.

using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // Library of scriptures
        List<Scripture> scriptures = new List<Scripture>
        {
            new Scripture(
                new Reference("John", 3, 16),
                "For God so loved the world that he gave his only begotten Son that whosoever believeth in him should not perish but have everlasting life."
            ),
            new Scripture(
                new Reference("Proverbs", 3, 5, 6),
                "Trust in the Lord with all thine heart and lean not unto thine own understanding. In all thy ways acknowledge him and he shall direct thy paths."
            )
        };

        Random rand = new Random();
        Scripture scripture = scriptures[rand.Next(scriptures.Count)];

        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine();
            Console.Write("Press enter to continue or type 'quit' to finish: ");

            string input = Console.ReadLine();
            if (input != null && input.ToLower() == "quit")
            {
                break;
            }

            scripture.HideRandomWords(3);

            if (scripture.AllWordsHidden())
            {
                Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                break;
            }
        }
    }
}

class Scripture
{
    private Reference _reference;
    private List<Word> _words;
    private Random _rand = new Random();

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(w => new Word(w)).ToList();
    }

    public void HideRandomWords(int count)
    {
        List<Word> visibleWords = _words.Where(w => !w.IsHidden()).ToList();

        for (int i = 0; i < count && visibleWords.Count > 0; i++)
        {
            int index = _rand.Next(visibleWords.Count);
            visibleWords[index].Hide();
            visibleWords.RemoveAt(index);
        }
    }

    public bool AllWordsHidden()
    {
        return _words.All(w => w.IsHidden());
    }

    public string GetDisplayText()
    {
        string text = string.Join(" ", _words.Select(w => w.GetDisplayText()));
        return $"{_reference.GetDisplayText()}\n{text}";
    }
}

class Reference
{
    private string _book;
    private int _chapter;
    private int _startVerse;
    private int? _endVerse;

    // Constructor for single verse
    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = verse;
        _endVerse = null;
    }

    // Constructor for verse range
    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = startVerse;
        _endVerse = endVerse;
    }

    public string GetDisplayText()
    {
        if (_endVerse == null)
        {
            return $"{_book} {_chapter}:{_startVerse}";
        }
        else
        {
            return $"{_book} {_chapter}:{_startVerse}-{_endVerse}";
        }
    }
}

class Word
{
    private string _text;
    private bool _hidden;

    public Word(string text)
    {
        _text = text;
        _hidden = false;
    }

    public void Hide()
    {
        _hidden = true;
    }

    public bool IsHidden()
    {
        return _hidden;
    }

    public string GetDisplayText()
    {
        if (_hidden)
        {
            return new string('_', _text.Length);
        }
        return _text;
    }
}

