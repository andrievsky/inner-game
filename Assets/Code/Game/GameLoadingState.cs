using Code.Common.StateMachine;
using Code.Logger;

namespace Code.Game
{
    public class GameLoadingState : AbstractState
    {
        private ILog _logger;

        public GameLoadingState(ILog logger)
        {
            _logger = logger;
        }

        public override void DidEnter(IState state)
        {
            _logger.Log("Loading Complete");
            StateMachine.Enter(typeof(GameSimulationState));
        }

        public override void Update(int deltaTime)
        {
        }

        public override void WillExit(IState state)
        {
        }
    }
}