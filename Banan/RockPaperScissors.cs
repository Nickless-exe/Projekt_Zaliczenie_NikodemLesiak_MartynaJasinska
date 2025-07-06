using System;
using System.Collections.Generic;

namespace Banan
{
    // Handles the Rock-Paper-Scissors mini-game logic
    class RockPaperScissorsGame
    {
        private Level currentLevel;
        private Action<Level, string[]> writeInBox;
        private int tutorialCount;

        public RockPaperScissorsGame(Level level, Action<Level, string[]> writeInBoxMethod, int tutorialCount)
        {
            currentLevel = level;
            writeInBox = writeInBoxMethod;
            this.tutorialCount = tutorialCount;
        }

        public bool Play()
        {
            writeInBox(currentLevel, new string[]
            {
                "Let's play Rock-Paper-Scissors!",
                "Choose your move:",
                "[1] Rock",
                "[2] Paper",
                "[3] Scissors"
            });

            Dictionary<int, string> moves = new Dictionary<int, string>
            {
                {1, "Rock"},
                {2, "Paper"},
                {3, "Scissors"}
            };

            int playerChoice = 0;
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1) playerChoice = 1;
                else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2) playerChoice = 2;
                else if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3) playerChoice = 3;

                if (playerChoice != 0) break;
            }

            Random rng = new Random();
            int npcChoice = rng.Next(1, 4); // 1 to 3

            writeInBox(currentLevel, new string[]
            {
                $"You chose: {moves[playerChoice]}",
                $"Konrad chose: {moves[npcChoice]}"
            });

            if (playerChoice == npcChoice)
            {
                writeInBox(currentLevel, new string[]
                {
                    "It's a tie! Play again...",
                    "Press any key to continue..."
                });
                Console.ReadKey(true);
                return Play();
            }

            // Automatic loss if the player has too few tutorials
            if (tutorialCount < 3)
            {
                writeInBox(currentLevel, new string[]
                {
                    "You lose! (Not enough tutorials)",
                    "Press any key to continue..."
                });
                Console.ReadKey(true);
                return false;
            }

            bool playerWins =
                (playerChoice == 1 && npcChoice == 3) ||
                (playerChoice == 2 && npcChoice == 1) ||
                (playerChoice == 3 && npcChoice == 2);

            if (playerWins)
            {
                writeInBox(currentLevel, new string[]
                {
                    "You win!",
                    "Press any key to continue..."
                });
                Console.ReadKey(true);
                return true;
            }
            else
            {
                writeInBox(currentLevel, new string[]
                {
                    "You lose!",
                    "Press any key to continue..."
                });
                Console.ReadKey(true);
                return false;
            }
        }
    }
}
