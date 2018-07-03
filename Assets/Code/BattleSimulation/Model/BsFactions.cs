using System;
using Code.BattleSimulation.Actor;

namespace Code.BattleSimulation.Model
{
    public interface IBsFactions : IBsSubModel
    {
        bool AreAllies(IBsActor actor1, IBsActor actor2);
        bool Contains(IBsActor actor, BsFaction faction);
        BsFaction Factions(IBsActor actor);
        void Add(IBsActor actor, BsFaction faction);
        void Remove(IBsActor actor, BsFaction faction);
    }

    public interface IBsFactioner : IBsSubModel
    {
        void Join(IBsActor actor, BsFaction faction, BsActionResult res);
        void Leave(IBsActor actor, BsFaction faction, BsActionResult res);
    }

    [Flags]
    public enum BsFaction : short
    {
        None = 0x0,
        Red = 0x1,
        Green = 0x2,
        Blue = 0x4,
    }

    // Factions model use hardcoded groups for simplicity
    public class BsFactions : BsActorModel<BsFaction>, IBsFactions, IBsFactioner, IBsSubModel
    {
        public BsFactions(IBsActorCollection actors) : base(actors, Build)
        {
        }

        public bool AreAllies(IBsActor actor1, IBsActor actor2)
        {
            return actor1 == actor2 || (Value(actor1) & Value(actor2)) != 0;
        }

        public BsFaction Factions(IBsActor actor)
        {
            return Value(actor);
        }

        public bool Contains(IBsActor actor, BsFaction faction)
        {
            return (Value(actor) & faction) != 0;
        }

        public void Add(IBsActor actor, BsFaction faction)
        {
            SetValue(actor, Value(actor) | faction);
        }

        public void Remove(IBsActor actor, BsFaction faction)
        {
            SetValue(actor, Value(actor) & ~faction);
        }

        public void Join(IBsActor actor, BsFaction faction, BsActionResult res)
        {
            if (Contains(actor, faction))
            {
                res.Reason = "Cann't join the same fraction twice";
                return;
            }

            Add(actor, faction);
            res.Reason = "Joined new fraction";
            res.Ok = true;
        }

        public void Leave(IBsActor actor, BsFaction faction, BsActionResult res)
        {
            if (!Contains(actor, faction))
            {
                res.Reason = "Join a faction first";
                return;
            }
            
            Remove(actor, faction);
            res.Reason = "Left some fraction";
            res.Ok = true;
        }

        private static BsFaction Build(IBsActor actor)
        {
            return BsFaction.None;
        }
    }
}