public class BankInteractor : Interactor
{
    private BankDataProvider dataProvider;
    private DayProgressInteractor progressInteractor;
    public float MoneyAmount => dataProvider.Money;
    public float DayProfit => dataProvider.DayProfit;

    public override void Init()
    {
        Bank.Init(this);
    }

    public override void OnCreate()
    {
        dataProvider = Core.DataProviders.GetDataProvider<BankDataProvider>();
        progressInteractor = Core.Interactors.GetInteractor<DayProgressInteractor>();
    }

    public bool Has(float amount)
    {
        return dataProvider.Money >= amount;
    }

    public void AddCoins(object sender, float  amount)
    {
        dataProvider.Money += amount;
        dataProvider.DayProfit += amount;

        progressInteractor.ChangeRating((int)amount);
    }

    public void Spend(object sender, float amount)
    {
        dataProvider.Money -= amount;
        dataProvider.DayProfit -= amount;
    }

    public void ResetDay()
    {
        dataProvider.DayProfit = 0;
    }
}
