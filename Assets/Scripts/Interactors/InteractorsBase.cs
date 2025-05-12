using System;
using System.Collections.Generic;

public class InteractorsBase 
{
    private Dictionary<Type, Interactor> interactorsMap;

    public InteractorsBase()
    {
        interactorsMap = new Dictionary<Type, Interactor>();
    }

    public void CreateAllInteractors()
    {
        CreateInteractor<BankInteractor>();
        CreateInteractor<StoragePlacementInteractor>();
        CreateInteractor<ProductsSpawnInteractor>();
        CreateInteractor<DayProgressInteractor>();
        CreateInteractor<QuestInteractor>();
        CreateInteractor<StatisticInteractor>();
        CreateInteractor<DeliveryInteractor>();
        CreateInteractor<PricingInteractor>();
    }

    private void CreateInteractor<T>() where T : Interactor, new()
    {
        var interactor = new T();
        var type = typeof(T);

        interactorsMap[type] = interactor;
    }

    public void SendOnCreateAllInteractors()
    {
        var allInteractors = interactorsMap.Values;

        foreach (var interactor in allInteractors)
        {
            interactor.OnCreate();
        }
    }
    public void InitializeAllInteractors()
    {
        var allInteractors = interactorsMap.Values;

        foreach (var interactor in allInteractors)
        {
            interactor.Init();
        }
    }

    public void SendOnStartAllInteractors()
    {
        var allInteractors = interactorsMap.Values;

        foreach (var interactor in allInteractors)
        {
            interactor.OnStart();
        }
    }

    public T GetInteractor<T>() where T : Interactor
    {
        var type = typeof(T);
        return (T) interactorsMap[type];
    }
}
