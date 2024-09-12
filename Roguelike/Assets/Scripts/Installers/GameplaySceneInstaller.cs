using Zenject;

namespace Roguelike.Installers
{
  public class GameplaySceneInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle().NonLazy();

      Container.Bind<RoomManager>().FromComponentInHierarchy().AsSingle().NonLazy();

      Container.Bind<Character>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
  }
}