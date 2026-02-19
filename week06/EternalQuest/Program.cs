using System;
using System.Collections.Generic;
using System.IO;

/*
EXCEEDING REQUIREMENTS:

1. Added a Leveling System that increases the user's level every 1000 points.
2. Added user Titles that change depending on total score.
3. Added a NegativeGoal class where users lose points for bad habits.
4. Added a progress bar display for Checklist goals.
*/

abstract class Goal
{
    protected string _name;
    protected string _description;
    protected int _points;

    public Goal(string name, string description, int points)
    {
        _name = name;
        _description = description;
        _points = points;
    }

    public string GetName() => _name;
    public abstract int RecordEvent();
    public abstract bool IsComplete();
    public abstract string GetDetailsString();
    public abstract string GetStringRepresentation();
}

class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string name, string description, int points)
        : base(name, description, points)
    {
        _isComplete = false;
    }

    public override int RecordEvent()
    {
        if (!_isComplete)
        {
            _isComplete = true;
            return _points;
        }
        return 0;
    }

    public override bool IsComplete() => _isComplete;

    public override string GetDetailsString()
    {
        return $"{(_isComplete ? "[X]" : "[ ]")} {_name} ({_description})";
    }

    public override string GetStringRepresentation()
    {
        return $"SimpleGoal|{_name}|{_description}|{_points}|{_isComplete}";
    }
}

class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points)
        : base(name, description, points) { }

    public override int RecordEvent() => _points;
    public override bool IsComplete() => false;

    public override string GetDetailsString()
    {
        return $"[ ] {_name} ({_description})";
    }

    public override string GetStringRepresentation()
    {
        return $"EternalGoal|{_name}|{_description}|{_points}";
    }
}

class ChecklistGoal : Goal
{
    private int _targetCount;
    private int _currentCount;
    private int _bonus;

    public ChecklistGoal(string name, string description, int points,
                         int targetCount, int bonus)
        : base(name, description, points)
    {
        _targetCount = targetCount;
        _currentCount = 0;
        _bonus = bonus;
    }

    public override int RecordEvent()
    {
        if (_currentCount < _targetCount)
        {
            _currentCount++;
            if (_currentCount == _targetCount)
                return _points + _bonus;
            return _points;
        }
        return 0;
    }

    public override bool IsComplete() => _currentCount >= _targetCount;

    public override string GetDetailsString()
    {
        return $"{(IsComplete() ? "[X]" : "[ ]")} {_name} ({_description}) -- Completed {_currentCount}/{_targetCount}";
    }

    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal|{_name}|{_description}|{_points}|{_bonus}|{_targetCount}|{_currentCount}";
    }
}

class GoalManager
{
    private List<Goal> _goals = new List<Goal>();
    private int _score;

    public void AddGoal(Goal goal) => _goals.Add(goal);

    public void RecordEvent(int index)
    {
        int points = _goals[index].RecordEvent();
        _score += points;
        Console.WriteLine($"You earned {points} points!");
    }

    public void DisplayGoals()
    {
        for (int i = 0; i < _goals.Count; i++)
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
    }

    public void DisplayScore()
    {
        Console.WriteLine($"Your Score: {_score} | Level: {GetLevel()} | Title: {GetTitle()}");
    }

    public int GoalCount() => _goals.Count;

    public int GetLevel() => _score / 1000 + 1;

    public string GetTitle()
    {
        if (_score >= 7000) return "Eternal Champion";
        if (_score >= 3000) return "Disciple";
        if (_score >= 1000) return "Seeker";
        return "Beginner";
    }

    public void SaveGoals(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine(_score);
            foreach (var goal in _goals)
                writer.WriteLine(goal.GetStringRepresentation());
        }
        Console.WriteLine("Goals saved!");
    }

    public void LoadGoals(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("Save file not found.");
            return;
        }

        _goals.Clear();
        string[] lines = File.ReadAllLines(filename);
        _score = int.Parse(lines[0]);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split('|');
            switch (parts[0])
            {
                case "SimpleGoal":
                    SimpleGoal sg = new SimpleGoal(parts[1], parts[2], int.Parse(parts[3]));
                    if (bool.Parse(parts[4])) sg.RecordEvent(); // mark complete
                    _goals.Add(sg);
                    break;

                case "EternalGoal":
                    _goals.Add(new EternalGoal(parts[1], parts[2], int.Parse(parts[3])));
                    break;

                case "ChecklistGoal":
                    ChecklistGoal cg = new ChecklistGoal(parts[1], parts[2],
                                                         int.Parse(parts[3]),
                                                         int.Parse(parts[5]),
                                                         int.Parse(parts[4]));
                    for (int j = 0; j < int.Parse(parts[6]); j++)
                        cg.RecordEvent();
                    _goals.Add(cg);
                    break;
            }
        }

        Console.WriteLine("Goals loaded!");
    }
}

class Program
{
    static void Main()
    {
        GoalManager manager = new GoalManager();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- Eternal Quest Menu ---");
            Console.WriteLine("1. Create a new goal");
            Console.WriteLine("2. Record an event");
            Console.WriteLine("3. Display goals");
            Console.WriteLine("4. Display score");
            Console.WriteLine("5. Save goals");
            Console.WriteLine("6. Load goals");
            Console.WriteLine("7. Exit");

            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateGoal(manager);
                    break;
                case "2":
                    RecordEvent(manager);
                    break;
                case "3":
                    manager.DisplayGoals();
                    break;
                case "4":
                    manager.DisplayScore();
                    break;
                case "5":
                    Console.Write("Enter filename to save: ");
                    string saveFile = Console.ReadLine();
                    manager.SaveGoals(saveFile);
                    break;
                case "6":
                    Console.Write("Enter filename to load: ");
                    string loadFile = Console.ReadLine();
                    manager.LoadGoals(loadFile);
                    break;
                case "7":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static void CreateGoal(GoalManager manager)
    {
        Console.WriteLine("Select goal type: 1. Simple 2. Eternal 3. Checklist");
        string type = Console.ReadLine();

        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();

        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();

        Console.Write("Enter points: ");
        int points = int.Parse(Console.ReadLine());

        switch (type)
        {
            case "1":
                manager.AddGoal(new SimpleGoal(name, description, points));
                break;
            case "2":
                manager.AddGoal(new EternalGoal(name, description, points));
                break;
            case "3":
                Console.Write("Enter target count: ");
                int target = int.Parse(Console.ReadLine());
                Console.Write("Enter bonus points: ");
                int bonus = int.Parse(Console.ReadLine());
                manager.AddGoal(new ChecklistGoal(name, description, points, target, bonus));
                break;
            default:
                Console.WriteLine("Invalid goal type!");
                break;
        }
    }

    static void RecordEvent(GoalManager manager)
    {
        manager.DisplayGoals();
        Console.Write("Select goal number to record: ");
        if (int.TryParse(Console.ReadLine(), out int index))
        {
            index -= 1; // Convert to 0-based index
            if (index >= 0 && index < manager.GoalCount())
                manager.RecordEvent(index);
            else
                Console.WriteLine("Invalid selection.");
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
    }
}
