using Code.Game;
using Zenject;

namespace Code.Startup
{
    public class StartupController : MonoInstaller
    {
        public override void InstallBindings()
        {
            GameInstaller.Install(Container);
        }
    }
}