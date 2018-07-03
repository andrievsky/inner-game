using System;
using Code.BattleSimulation;
using Code.BattleSimulation.View;
using Code.Common.StateMachine;
using Code.Input;
using Code.Logger;
using Zenject;

namespace Code.Game
{
    public class GameAutoInstaller : IAutoInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.Bind<IInitializable>().To<GameController>().AsSingle();
        }
    }
    
    public class GameController : IInitializable, IDisposable
    {
        private readonly ILog _logger;
        private readonly IBsView _view;
        private readonly IBsHudView _hudView;
        private readonly IInputController _input;
        private readonly GameConfig _config;
        private readonly SyncStateMachine _stateMachine = new SyncStateMachine();

        public GameController(ILog logger, IBsView view, IBsHudView hudView, IInputController input, GameConfig config)
        {
            _logger = logger;
            _view = view;
            _hudView = hudView;
            _input = input;
            _config = config;
        }

        public void Initialize()
        {
            _logger.Log("GameController Ready");
            // Instantiate states with DI Factory later
            _stateMachine.Init(new IState[]{new GameLoadingState(_logger), new GameSimulationState(_logger, _view, _hudView, _input, _config)});
        }

        public void Dispose()
        {
            _logger.Log("GameController Disposed");
        }
    }
}
