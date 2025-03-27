using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private ProductFinder productFinder;
    public override void InstallBindings()
    {
        Container.Bind<ProductFinder>().FromInstance(productFinder);
        Container.Bind<SettingsDataProvider>().AsSingle();
        Container.Bind<StoreFurnitureConfigFinder>().AsSingle();
        Container.Bind<QuestConfigFinder>().AsSingle();
    }
}