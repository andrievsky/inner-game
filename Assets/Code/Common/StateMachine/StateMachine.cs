using System;

namespace Code.Common.StateMachine
{
    // Inspired by GKStateMachine https://developer.apple.com/documentation/gameplaykit/gkstatemachine
    // Minimal impl. for simplicity
    // Could be done more generic
    public interface IStateMachine<T> where T : IState
    {
        void Init(T[] states);
        T CurrentState { get; }
        bool CanEnterState(Type type);
        bool Enter(Type type);
        void Update(int deltaTime);
    }

    public class SyncStateMachine : IStateMachine<IState>, IDisposable
    {
        #region State
        private IState[] _states;
        #endregion

        #region Props
        public IState CurrentState { get; private set; }
        #endregion

        #region Lifecycle
        public void Init(IState[] states)
        {
            if (states == null)
            {
                throw new ArgumentException("states");
            }
            if (states.Length == 0)
            {
                throw new ArgumentException("At least one state should be available");
            }
            if (_states != null)
            {
                throw new Exception("State machime has been already init");
            }

            foreach (var state in states)
            {
                state.StateMachine = this;
            }
            _states = states;
            CurrentState = states[0];
            CurrentState.DidEnter(null);
        }
        
        public void Dispose()
        {
            if (CurrentState != null)
            {
                CurrentState.WillExit(null);
                CurrentState = null;
            }
            
            _states = null;
        }
        #endregion

        #region Public
        public bool CanEnterState(Type type)
        {
            return CurrentState.IsValidNextState(type);
        }

        public bool Enter(Type type)
        {
            if (!CanEnterState(type))
            {
                return false;
            }
            
            IState next = null;
            foreach (var state in _states)
            {
                if (state.GetType() != type)
                {
                    continue;
                }
                next = state;
                break;
            }

            if (next == null)
            {
                return false;
            }
            
            var prev = CurrentState;
            CurrentState.WillExit(next);
            CurrentState = next;
            CurrentState.DidEnter(prev);
            return true;
        }

        public void Update(int deltaTime)
        {
            CurrentState.Update(deltaTime);
        }
        #endregion
    }
}
