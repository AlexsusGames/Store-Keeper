using System;

public static class Bank
{
    public static event Action OnBankInitialized;
    public static event Action OnChanged;

    private static BankInteractor bankInteractor;
    public static float MoneyAmount
    {
        get
        {
            CheckInit();
            return bankInteractor.MoneyAmount;
        }
    }

    public static float DayProfit
    {
        get
        {
            CheckInit();
            return bankInteractor.DayProfit;
        }
    }

    public static void OnDayEnd()
    {
        Core.Statistic.OnDayPassed(DayProfit);
        bankInteractor.ResetDay();
    }

    public static bool isInitialized { get; private set; }

    public static void Init(BankInteractor interactor)
    {
        bankInteractor = interactor;
        isInitialized = true;

        OnBankInitialized?.Invoke();
    }

    public static bool Has(float amount)
    {
        CheckInit();
        return bankInteractor.Has(amount);
    }

    public static void AddCoins(object sender, float amount)
    {
        CheckInit();
        bankInteractor.AddCoins(sender, amount);
        OnChanged?.Invoke();
    }

    public static void Spend(object sender, float amount)
    {
        CheckInit();
        bankInteractor.Spend(sender, amount);
        OnChanged?.Invoke();
    }

    private static void CheckInit()
    {
        if (!isInitialized)
        {
            throw new Exception("Bank is not initialized");
        }
    }
}
