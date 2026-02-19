using System;
using System.Collections.Generic;

// Base class
public abstract class Activity
{
    private DateTime _date;
    private int _length; // in minutes

    public Activity(DateTime date, int length)
    {
        _date = date;
        _length = length;
    }

    public DateTime Date => _date;
    public int Length => _length;

    public abstract double GetDistance(); // km
    public abstract double GetSpeed();    // km/h
    public abstract double GetPace();     // min per km

    public virtual string GetSummary()
    {
        string activityName = this.GetType().Name;
        double distance = GetDistance();
        double speed = GetSpeed();
        double pace = GetPace();

        return $"{_date:dd MMM yyyy} {activityName} ({_length} min) - " +
               $"Distance {distance:F1} km, Speed {speed:F1} kph, Pace: {pace:F2} min per km";
    }
}

// Derived class: Running
public class Running : Activity
{
    private double _distance; // km

    public Running(DateTime date, int length, double distance)
        : base(date, length)
    {
        _distance = distance;
    }

    public override double GetDistance() => _distance;

    public override double GetSpeed() => (_distance / Length) * 60;

    public override double GetPace() => Length / _distance;
}

// Derived class: Cycling
public class Cycling : Activity
{
    private double _speed; // km/h

    public Cycling(DateTime date, int length, double speed)
        : base(date, length)
    {
        _speed = speed;
    }

    public override double GetDistance() => _speed * Length / 60;

    public override double GetSpeed() => _speed;

    public override double GetPace() => 60 / _speed;
}

// Derived class: Swimming
public class Swimming : Activity
{
    private int _laps;

    public Swimming(DateTime date, int length, int laps)
        : base(date, length)
    {
        _laps = laps;
    }

    public override double GetDistance() => (_laps * 50) / 1000.0; // km

    public override double GetSpeed() => (GetDistance() / Length) * 60;

    public override double GetPace() => Length / GetDistance();
}

// Program
class Program
{
    static void Main()
    {
        List<Activity> activities = new List<Activity>
        {
            new Running(DateTime.Parse("2022-11-03"), 30, 4.8),
            new Cycling(DateTime.Parse("2022-11-03"), 45, 20),
            new Swimming(DateTime.Parse("2022-11-03"), 60, 40)
        };

        foreach (Activity activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
