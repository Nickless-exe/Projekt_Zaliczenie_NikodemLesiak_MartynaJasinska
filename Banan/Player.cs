namespace Banan
{
    class Player : Character
    {
        public Inventory inventory = new Inventory();
        

        Dictionary<ConsoleKey, string> keyMap = new Dictionary<ConsoleKey, string>
        {
            { ConsoleKey.A, "moveLeft" },
            { ConsoleKey.D, "moveRight" },
            { ConsoleKey.W, "moveUp" },
            { ConsoleKey.S, "moveDown" },
            { ConsoleKey.E, "inventory" }
        };

        public string chosenAction = "pass";

        public Player(string name, string avatar) : base(name, avatar) { }

        public override string ChooseAction()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            chosenAction = keyMap.GetValueOrDefault(key.Key, "pass");
            return chosenAction;
        }
    }
}
