using Zenject;
using UnityEngine;
using MiniIT.InputSystem;

namespace MiniIT.Managers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private MobileInputConfig mobileInputConfig = null;
        [SerializeField] private CharactersSpawner charactersSpawner = null;
        [SerializeField] private InputPanel inputPanel = null;

        public override void InstallBindings()
        {
            if (Application.isMobilePlatform)
            {
                Container.Bind<MobileInputConfig>()
             .FromScriptableObject(mobileInputConfig)
             .AsSingle();

                Container.Bind<IInputService>()
                         .To<MobileInputService>()
                         .AsSingle()
                         .NonLazy();
            }
            else
            {
                Container.Bind<IInputService>()
                         .To<DesktopInputService>()
                         .AsSingle()
                         .NonLazy();
            }

            Container.Bind<CharactersSpawner>().FromInstance(charactersSpawner).AsSingle();
            Container.Bind<InputPanel>().FromInstance(inputPanel).AsSingle();
        }
    }
}