namespace Banan;

    class NonPlayerCharacter : Character
{
    public bool IsActive { get; set; } = true;
    private Random random = new Random();

    private int actionCounter = 0;

    public NonPlayerCharacter(string name, string symbol) : base(name, symbol) { }

    public override string ChooseAction()
    {
        if (!IsActive)
            return "";

        actionCounter++;

        if (actionCounter < 2)
        {
            return "";
        }
        else
        {
            actionCounter = 0;

            string[] moves = { "moveLeft", "moveRight", "moveUp", "moveDown" };
            int index = random.Next(moves.Length);
            return moves[index];
        }
    }
}
