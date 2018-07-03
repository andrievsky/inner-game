using System;

namespace Code.Common.StateMachine
{
    // Inspired by GKStateMachine https://developer.apple.com/documentation/gameplaykit/gkstatemachine
    // Minimal impl. for simplicity
    // Could be done more generic
    public interface IState
    {
        bool IsValidNextState(Type type);
        IStateMachine<IState> StateMachine { get; set; }
        void DidEnter(IState state);
        void Update(int deltaTime);
        void WillExit(IState state);
    }

    public abstract class AbstractState : IState
    {
        private IStateMachine<IState> _machine;

        internal void Init(IStateMachine<IState> machine)
        {
            _machine = machine;
        }

        public bool IsValidNextState(Type type)
        {
            return true;
        }

        public IStateMachine<IState> StateMachine
        {
            get { return _machine; }
            set
            {
                if (value != null && value != _machine)
                {
                    _machine = value;
                }
            }
        }

        public abstract void DidEnter(IState state);

        public abstract void Update(int deltaTime);

        public abstract void WillExit(IState state);
    }
}
