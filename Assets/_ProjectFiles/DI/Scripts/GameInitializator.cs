using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private FirstPersonParameters _firstPersonParameters;

    public override void InstallBindings()
    {
        Container.Bind<FirstPersonMovement>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<RotatableItemsController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<ItemMenu>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<Speed>().AsSingle().NonLazy();
        Container.Bind<FirstPersonParameters>().FromInstance(_firstPersonParameters).AsSingle().NonLazy();
        Container.Bind<InputHandler>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<HintUI>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerItemsHandler>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<DialogueUIManager>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
