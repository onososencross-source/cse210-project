using System;

class Program
{
    static void Main()
    {
        // Stretch: allow replaying the game
        string playAgain = "yes";

        while (playAgain.ToLower() == "yes")
        {
            // Core Requirement 3: generate random magic number (1–100)
            Random random = new Random();
            int magicNumber = random.Next(1, 101);

            int guess = -1;
            int guessCount = 0; // Stretch: count guesses

            // Core Requirement 2: loop until guess matches magic number
            while (guess != magicNumber)
            {
                Console.Write("What is your guess? ");
                guess = int.Parse(Console.ReadLine());
                guessCount++;

                if (guess < magicNumber)
                {
                    Console.WriteLine("Higher");
                }
                else if (guess > magicNumber)
                {
                    Console.WriteLine("Lower");
                }
                else
                {
                    Console.WriteLine("You guessed it!");
                }
            }

            // Stretch: show number of guesses
            Console.WriteLine($"It took you {guessCount} guesses.");

            // Stretch: ask to play again
            Console.Write("Do you want to play again? (yes/no) ");
            playAgain = Console.ReadLine();
        }
    }
}
