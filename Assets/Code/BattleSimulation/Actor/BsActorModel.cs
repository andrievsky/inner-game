using System;
using System.Collections.Generic;
using Code.Extensions;

namespace Code.BattleSimulation.Actor
{
    public abstract class BsActorModel<T> : IDisposable
    {
        private event Action<T> OnAdd;
        public readonly IList<T> Data = new List<T>();
        private readonly IBsActorCollection _actors;
        private readonly Func<IBsActor, T> _factory;

        protected BsActorModel(IBsActorCollection actors, Func<IBsActor, T> factory)
        {
            _actors = actors;
            _factory = factory;
            for (int i = 0; i < actors.Count(); i++)
            {
                Add(actors.Actor(i));
            }

            _actors.OnAdd += OnAddHandler;
        }

        private void OnAddHandler(IBsActor actor)
        {
            Add(actor);
            OnAdd.Dispatch(Data[Data.Count - 1]);
        }

        private void Add(IBsActor actor)
        {
            Data.Add(_factory(actor));
        }

        public void Dispose()
        {
            Data.Clear();
            _actors.OnAdd -= OnAddHandler;
        }

        public T Value(IBsActor actor)
        {
            return Data[actor.Id()];
        }

        public void SetValue(IBsActor actor, T value)
        {
            Data[actor.Id()] = value;
        }
    }
}