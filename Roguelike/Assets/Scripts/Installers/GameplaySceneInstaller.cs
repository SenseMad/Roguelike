using Zenject;

namespace Roguelike.Installers
{
  public class GameplaySceneInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      //Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
  }
}