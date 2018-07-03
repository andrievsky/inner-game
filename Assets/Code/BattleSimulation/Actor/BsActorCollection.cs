using System;
using System.Collections.Generic;
using Code.BattleSimulation.Config;
using Code.Extensions;

namespace Code.BattleSimulation.Actor
{
    public interface IBsActorCollection
    {
        event Action<IBsActor> OnAdd;
        IBsActor Actor(int index);
        int Count();
    }

    public class BsActorCollection : IBsActorCollection
    {
        public event Action<IBsActor> OnAdd;

        private readonly IList<IBsActor> _actors = new List<IBsActor>();

        public IBsActor Add(IBsActorConfig config)
        {
            var actor = new BsActor(_actors.Count, config);
            _actors.Add(actor);
            OnAdd.Dispatch(actor);
            return actor;
        }

        public IBsActor Actor(int index)
        {
            return _actors[index];
        }

        public int Count()
        {
            return _actors.Count;
        }
    }
}