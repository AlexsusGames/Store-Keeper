public class BankInteractor : Interactor
{
    private BankDataProvider dataProvider;
    public int MoneyAmount => dataProvider.money;

    public override void Init()
    {
        Bank.Init(this);
    }

    public override void OnCreate()
    {
        dataProvider = Core.DataProviders.GetDataProvider<BankDataProvider>();
    }

    public bool Has(int amount)
    {
        return dataProvider.money >= amount;
    }

    public void AddCoins(object sender, int  amount)
    {
        dataProvider.money += amount;
    }

    public void Spend(object sender, int amount)
    {
        dataProvider.money -= amount;
    }
}
