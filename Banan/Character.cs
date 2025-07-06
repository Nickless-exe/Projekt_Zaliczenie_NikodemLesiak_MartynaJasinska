
namespace Banan
{
    // Abstract base class for all characters (player and NPCs) in the game
    public abstract class Character
    {
        public string name;      // Character's name
        public Point position;   // Current position on the map
        public int speed = 1;    // Movement speed (cells per move)
        public string avatar;    // Character's display symbol

        // Constructor: sets name and avatar
        public Character(string name, string avatar)
        {
            this.name = name;
            this.avatar = avatar;
        }

        // Moves the character in the given direction, stopping at walls
        public void Move(Point direction, Level level)
        {
            Point target = position;

            // Move horizontally (X axis)
            int signX = Math.Sign(direction.x);
            for (int x = 1; x <= Math.Abs(direction.x * speed); x++)
            {
                int coordinateToTest = position.x + x * signX;
                // Stop if hitting a wall
                if (level.GetCellVisuals(coordinateToTest, target.y) == '#') break;
                target.x = coordinateToTest;
            }

            // Move vertically (Y axis)
            int signY = Math.Sign(direction.y);
            for (int y = 1; y <= Math.Abs(direction.y * speed); y++)
            {
                int coordinateToTest = position.y + y * signY;
                // Stop if hitting a wall
                if (level.GetCellVisuals(target.x, coordinateToTest) == '#') break;
                target.y = coordinateToTest;
            }

            // Clamp position to map bounds
            target.y = Math.Clamp(target.y, 0, level.GetHeight() - 1);
            target.x = Math.Clamp(target.x, 0, level.GetRowWidth(target.y) - 1);

            // Only move if not hitting a wall
            if (level.GetCellVisuals(target.x, target.y) != '#')
            {
                position = target;
            }
        }

        // Draws the character at its current position
        public void Display()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(avatar);
        }

        // Each character must implement how it chooses its next action
        public abstract string ChooseAction();
    }
}
