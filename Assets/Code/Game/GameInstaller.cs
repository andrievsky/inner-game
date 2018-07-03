using System;
using System.Linq;
using Code.Logger;
using Zenject;

namespace Code.Game
{
    // IAutoInstaller used to provide a mechanism for features to install own dependecies
    // Which is a must for bigger projects to prevent context growing
    // And could be more navive way than scene contexts with prefabs/scriptableobjects installers
    public interface IAutoInstaller
    {
        void InstallBindings(DiContainer container);
    }

    public class GameInstaller : Installer<GameInstaller>
    {
        public override void InstallBindings()
        {
            // Automaticly install dependencies
            foreach (var t in GetBindableTypes<IAutoInstaller>())
            {
                ((IAutoInstaller) Activator.CreateInstance(t)).InstallBindings(Container);
            }
            Container.Bind<ILog>().To<Logger.Logger>().AsSingle();
        }

        private static Type[] GetBindableTypes<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToArray();
        }
    }
}