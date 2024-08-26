using Zenject;

namespace Roguelike.Installers
{
  public class GlobalInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      //Container.Bind<InputHandler>().FromNewComponentOnNewGameObject().AsSingle();
    }
  }
}