using System;
using Code.BattleSimulation.Actor;

namespace Code.BattleSimulation
{
    public interface IBsLevels : IBsSubModel
    {
        int Level(IBsActor actor);
        void AddLevel(IBsActor actor, int value);
    }

    public class BsLevels : BsActorModel<int>, IBsLevels
    {
        public BsLevels(IBsActorCollection actors) : base(actors, Build)
        {
        }

        private static int Build(IBsActor actor)
        {
            return 1;
        }

        public int Level(IBsActor actor)
        {
            return Value(actor);
        }

        public void AddLevel(IBsActor actor, int value)
        {
            Data[actor.Id()] = Math.Max(Value(actor) + value, 1);
        }
    }
}