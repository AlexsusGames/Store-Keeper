using System;

public static class Bank
{
    public static event Action OnBankInitialized;

    private static BankInteractor bankInteractor;
    public static int MoneyAmount
    {
        get
        {
            CheckInit();
            return bankInteractor.MoneyAmount;
        }
    }
    public static bool isInitialized { get; private set; }

    public static void Init(BankInteractor interactor)
    {
        bankInteractor = interactor;
        isInitialized = true;

        OnBankInitialized?.Invoke();
    }

    public static bool Has(int amount)
    {
        CheckInit();
        return bankInteractor.Has(amount);
    }

    public static void AddCoins(object sender, int amount)
    {
        CheckInit();
        bankInteractor.AddCoins(sender, amount);
    }

    public static void Spend(object sender, int amount)
    {
        CheckInit();
        bankInteractor.Spend(sender, amount);
    }

    private static void CheckInit()
    {
        if (!isInitialized)
        {
            throw new Exception("Bank is not initialized");
        }
    }
}
