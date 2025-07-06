namespace Banan;

    class CashierNPC : NonPlayerCharacter
{
    public CashierNPC(string name, string symbol) : base(name, symbol)
    {
        IsActive = false;
    }

    public override string ChooseAction()
    {
        return "";
    }

    public bool CanSellBanana(bool firstQuestCompleted)
    {
        return firstQuestCompleted;
    }

    public string SellBanana()
    {
        return "banana";
    }
}
