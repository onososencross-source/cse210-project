using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<Video> videos = new List<Video>();

        // Video 1
        Video v1 = new Video("Learning C# Basics", "CodeWithSam", 600);
        v1.AddComment(new Comment("Alex", "Great explanation!"));
        v1.AddComment(new Comment("Mira", "This helped me a lot."));
        v1.AddComment(new Comment("Jay", "Can you do a video on classes?"));
        videos.Add(v1);

        // Video 2
        Video v2 = new Video("Top 10 Football Goals", "SportsHub", 480);
        v2.AddComment(new Comment("Chris", "Goal #3 was insane!"));
        v2.AddComment(new Comment("Tobi", "Best compilation ever."));
        v2.AddComment(new Comment("Liam", "Messi is the GOAT."));
        videos.Add(v2);

        // Video 3
        Video v3 = new Video("Easy Jollof Rice Recipe", "KitchenQueen", 720);
        v3.AddComment(new Comment("Ada", "Tried this and loved it!"));
        v3.AddComment(new Comment("John", "Very simple to follow."));
        v3.AddComment(new Comment("Grace", "Please do fried rice next."));
        videos.Add(v3);

        // Display all videos
        foreach (Video video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length (seconds): {video.LengthSeconds}");
            Console.WriteLine($"Number of Comments: {video.GetCommentCount()}");

            Console.WriteLine("Comments:");
            foreach (Comment comment in video.GetComments())
            {
                Console.WriteLine($"  {comment.CommenterName}: {comment.Text}");
            }

            Console.WriteLine("-----------------------------------");
        }
    }
}
