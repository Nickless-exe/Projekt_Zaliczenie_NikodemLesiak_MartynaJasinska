using System;
using System.Collections.Generic;
using Banan;

namespace Banan
{
    class Program
    {
        static bool gameEnded = false;

        static int GetUIBoxStartY(Level level)
        {
            return level.GetHeight() + 2;
        }

        static void ClearInteractionBox(Level level, int height = 6)
        {
            int startY = GetUIBoxStartY(level);
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(0, startY + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
        }

        static void DrawInventoryHint(Level level)
        {
            int boxY = GetUIBoxStartY(level);
            Console.SetCursorPosition(0, boxY - 1);
            Console.WriteLine("[E] to open inventory");
        }

        static void WriteInBox(Level level, params string[] lines)
        {
            int boxY = GetUIBoxStartY(level);
            DrawInventoryHint(level);
            ClearInteractionBox(level);
            for (int i = 0; i < lines.Length && i < 6; i++)
            {
                Console.SetCursorPosition(0, boxY + i);
                Console.WriteLine(lines[i]);
            }
        }

        static void EndGame()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("And they lived happily ever after...");
            Console.WriteLine("Press [E] to exit.");

            gameEnded = true;

            while (true)
            {
                var endKey = Console.ReadKey(true).Key;
                if (endKey == ConsoleKey.E)
                    Environment.Exit(0);
            }
        }

        // Handles all interactions with the main NPC (Konrad Gadzina)
        static void HandleNpcInteraction(Player player, ref bool isInteracting, ref bool questAccepted, ref bool questCompleted, ref bool canBuyBanana, Level level)
        {
            // Check if player has a banana for quest completion
            bool hasBanana = player.inventory.HasItem("Banana");

            // If quest is completed and player has banana, show ending and challenge
            if (questCompleted && hasBanana)
            {
                WriteInBox(level,
                    "Konrad Gadzina: Ooo, you brought a banana? Awesome!",
                    "Mmmmm, Yummy.",
                    "But wait... before I pass you...",
                    "You have to beat me in Rock-Paper-Scissors!",
                    "Press SPACE to start the challenge.");

                while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }

                // Start the mini-game challenge
                var rpsGame = new RockPaperScissorsGame(level, WriteInBox, player.inventory.Count("Tutorial"));
                bool won = rpsGame.Play();

                // Allow retrying until the player wins
                while (!won)
                {
                    WriteInBox(level,
                        "Konrad Gadzina: Haha, not so fast!",
                        "Try again if you want to pass!",
                        "Press SPACE to play again...");
                    while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
                    won = rpsGame.Play();
                }
                EndGame();
            }

            // Show interaction options for the NPC
            WriteInBox(level,
                "NPC Interaction:",
                "[T]alk",
                "[F]ight",
                "[Q]uest",
                "[E]xit");

            // Main loop for handling NPC interaction choices
            while (true)
            {
                var key = Console.ReadKey(true).Key;

                // Exit interaction
                if (key == ConsoleKey.E)
                {
                    WriteInBox(level, "Konrad Gadzina: Ok, bye!");
                    isInteracting = false;
                    break;
                }
                // Talk to NPC
                else if (key == ConsoleKey.T)
                {
                    WriteInBox(level,
                        "Konrad Gadzina: Guys! Did you finish",
                        "your final project yet? I'm waiting......",
                        "Press SPACE to continue...");
                    while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
                    isInteracting = false;
                    break;
                }
                // Quest logic: accept, check progress, or complete
                else if (key == ConsoleKey.Q)
                {
                    if (!questAccepted)
                    {
                        WriteInBox(level,
                            "Konrad Gadzina: Welcome to an adventure",
                            "to become the greatest programmer!",
                            "Accept quest? [Y]es / [N]o");

                        var questKey = Console.ReadKey(true).Key;

                        if (questKey == ConsoleKey.Y)
                        {
                            WriteInBox(level,
                                "Quest accepted:",
                                "Konrad Gadzina: To achieve that, you have to watch at least three of my YT tutorials @MakeGamesToday!",
                                "Collect three tutorials: <X>. ",
                                "Press [E] to exit.");
                            questAccepted = true;
                        }
                        else if (questKey == ConsoleKey.N)
                        {
                            WriteInBox(level,
                                "Quest declined:",
                                "Konrad Gadzina: OK, bye looser. (respectfully)",
                                "Press [E] to exit.");
                        }
                    }
                    else
                    {
                        int tutorialsCount = player.inventory.Count("Tutorial");

                        if (tutorialsCount >= 3)
                        {
                            WriteInBox(level,
                                "Konrad Gadzina: I see you watched my tutorials!",
                                "You're on your way to becoming a real dev!",
                                "Congratulations! First part of the Quest complete.",
                                "Press SPACE to continue...");

                            while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }

                            questCompleted = true;
                            canBuyBanana = true;

                            WriteInBox(level,
                                "You: Sooooo, Have I passed your class already?",
                                "Press SPACE to continue...");

                            while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }

                            WriteInBox(level,
                                "Konrad Gadzina: Are you crazy?",
                                "Bring me a banana from Żabka first!",
                                "Press SPACE to continue...");
                            while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
                            isInteracting = false;
                            break;
                        }
                        else
                        {
                            WriteInBox(level,
                                $"Konrad Gadzina: You've only watched {tutorialsCount} tutorial(s)...",
                                "That's not enough! You need at least 3.",
                                "Come back when you're smarter.");
                        }
                    }
                }
                // Initiate Rock-Paper-Scissors mini-game (fight)
                else if (key == ConsoleKey.F)
                {
                    var rpsGame = new RockPaperScissorsGame(level, WriteInBox, player.inventory.Count("Tutorial"));
                    bool won = rpsGame.Play();

                    if (won)
                    {
                        WriteInBox(level,
                            "Konrad Gadzina: Not bad! You won the game!",
                            "Maybe you will pass after all...",
                            "Press [E] to exit.");
                    }
                    else
                    {
                        WriteInBox(level,
                            "Konrad Gadzina: This is all you've got?! Come back when you're ready, or something.",
                            "Press any key to exit...");
                        Console.ReadKey(true);
                        isInteracting = false;
                        break;
                    }
                }
            }
        }

        static void HandleCashierInteraction(Player player, ref bool bananaCollected, bool canBuyBanana, Level level)
        {
            WriteInBox(level,
                "NPC Interaction:",
                "Cashier: What can I get you?",
                "You: One Banana please.",
                "Press SPACE to continue...");

            while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }

            if (canBuyBanana && !bananaCollected)
            {
                WriteInBox(level, "Cashier: Here you go. Enjoy!");
                player.inventory.Add(new Item("Banana", "food"));
                bananaCollected = true;
            }
            else
            {
                WriteInBox(level, "Cashier: Sorry we don't have any...");
            }
        }

        static void Main(string[] args)
        {
            Level mainLevel = new Level();
            mainLevel.PlaceRandomItem('X', 4);

            Level zabkaLevel = new ZabkaLevel();
            Level currentLevel = mainLevel;

            bool questAccepted = false;
            bool questCompleted = false;
            bool bananaCollected = false;
            bool canBuyBanana = false;
            bool isInteracting = false;

            Console.CursorVisible = false;

            var directions = new Dictionary<string, Point>
            {
                ["moveLeft"] = new Point(-1, 0),
                ["moveRight"] = new Point(1, 0),
                ["moveUp"] = new Point(0, -1),
                ["moveDown"] = new Point(0, 1)
            };

            Point randomNpcPosition = mainLevel.GetRandomEmptyPosition();

            Player player = new Player("TheStudent", "@") { position = new Point(2, 2) };
            NonPlayerCharacter npc = new NonPlayerCharacter("KonradGadzina", "K") { position = randomNpcPosition };
            CashierNPC cashier = new CashierNPC("ZabkaCashier", "C") { position = new Point(20, 7) };

            List<Character> mainLevelCharacters = new List<Character> { player, npc };
            List<Character> zabkaLevelCharacters = new List<Character> { player, cashier };

            currentLevel.Display();
            DrawInventoryHint(currentLevel);
            foreach (var ch in mainLevelCharacters)
                ch.Display();

            while (true)
            {
                if (gameEnded) continue;

                List<Character> currentCharacters = currentLevel == mainLevel ? mainLevelCharacters : zabkaLevelCharacters;

                foreach (var ch in currentCharacters)
                    ch.Display();

                for (int i = 0; i < currentCharacters.Count; i++)
                {
                    Character ch = currentCharacters[i];
                    string action = isInteracting ? "interact" : ch.ChooseAction();

                    if (action == "inventory" && ch is Player currentPlayer)
                    {
                        int startY = GetUIBoxStartY(currentLevel);
                        currentPlayer.inventory.Show(startY);

                        while (true)
                        {
                            var key = Console.ReadKey(true).Key;
                            if (key == ConsoleKey.E) break;
                        }

                        ClearInteractionBox(currentLevel);
                        continue;
                    }

                    if (!directions.ContainsKey(action))
                        continue;

                    currentLevel.RedrawCell(ch.position);
                    ch.Move(directions[action], currentLevel);

                    char cell = currentLevel.GetCellVisuals(ch.position.x, ch.position.y);

                    if (ch is Player playerAtCell && cell == 'X')
                    {
                        if (questAccepted)
                        {
                            WriteInBox(currentLevel, "You have watched a tutorial! You smartass...");
                            playerAtCell.inventory.Add(new Item("Tutorial", "knowledge"));
                            currentLevel.SetCell(ch.position.x, ch.position.y, '.');
                        }
                        else
                        {
                            WriteInBox(currentLevel, "You don't feel like watching tutorials yet...", "Maybe someone should give you a reason?");
                        }
                    }

                    if (ch is Player playerEnter && cell == '>')
                    {
                        Console.Clear();
                        currentLevel = zabkaLevel;
                        playerEnter.position = new Point(1, 2);
                        DrawInventoryHint(currentLevel);
                        WriteInBox(currentLevel, "You enter Żabka...");
                        currentLevel.Display();
                        continue;
                    }
                    else if (ch is Player playerExit && cell == '<')
                    {
                        Console.Clear();
                        currentLevel = mainLevel;
                        playerExit.position = new Point(5, 5);
                        DrawInventoryHint(currentLevel);
                        WriteInBox(currentLevel, "You leave Żabka and return...");
                        currentLevel.Display();
                        continue;
                    }

                    // Check for cashier interaction in Żabka level
                    if (ch is Player playerInZabka && currentLevel == zabkaLevel)
                    {
                        // Loop through all characters in Żabka to find the cashier
                        foreach (var character in zabkaLevelCharacters)
                        {
                            // If the character is the cashier and is close enough, trigger interaction
                            if (character is CashierNPC cashierNpc)
                            {
                                // Manhattan distance check (<= 2)
                                if (Math.Abs(cashierNpc.position.x - playerInZabka.position.x) +
                                    Math.Abs(cashierNpc.position.y - playerInZabka.position.y) <= 2)
                                {
                                    HandleCashierInteraction(playerInZabka, ref bananaCollected, canBuyBanana, currentLevel);
                                }
                            }
                        }
                    }
                }

                // Check for NPC interaction trigger in the main level
                if (!isInteracting && player != null && currentLevel == mainLevel)
                {
                    // If the player is adjacent to the NPC, start interaction
                    if (Math.Abs(npc.position.x - player.position.x) +
                        Math.Abs(npc.position.y - player.position.y) <= 1)
                    {
                        isInteracting = true;
                        HandleNpcInteraction(player, ref isInteracting, ref questAccepted, ref questCompleted, ref canBuyBanana, currentLevel);
                    }
                }
            }
        }
    }
}
