using Code.Extensions;
using Code.Game;
using Zenject;

namespace Code.Input
{
    public class InputInstaller : IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.BindInterfacesTo<InputController>().AsSingle();
        }
    }

    public interface IInputController
    {
        void SetBinds(IKeyBindMap map);
        void ClearBinds();
    }

    public class InputController : IInputController, ITickable
    {
        private static readonly IKeyBindMap EmptyKeyMap = new KeyBindMap();
        private IKeyBindMap _map = EmptyKeyMap;

        public void SetBinds(IKeyBindMap map)
        {
            if (map == null)
            {
                return;
            }

            _map = map;
        }

        public void ClearBinds()
        {
            _map = EmptyKeyMap;
        }

        public void Tick()
        {
            foreach (var bind in _map.Binds())
            {
                if (!UnityEngine.Input.GetKeyDown(bind.Key))
                {
                    continue;
                }

                bind.Callback.Dispatch();
            }
        }
    }
}