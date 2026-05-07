using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<RotatableItemsController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<ItemMenu>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<InputHandler>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<HintUI>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerItemsHandler>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
