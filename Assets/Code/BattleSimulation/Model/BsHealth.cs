using System;
using Code.BattleSimulation.Actor;

namespace Code.BattleSimulation.Model
{
    public interface IBsHealth : IBsSubModel
    {
        int Health(IBsActor actor);
        bool IsDead(IBsActor actor);
    }

    public interface IBsHealer : IBsSubModel
    {
        void Heal(IBsActor source, IBsActor target, BsActionResult res);
    }

    public interface IBsAttacker : IBsSubModel
    {
        void Attack(IBsActor source, IBsActor target, BsActionResult res);
    }

    public class BsHealthModel : BsActorModel<int>, IBsHealth, IBsHealer, IBsAttacker
    {
        private readonly IBsStats _stats;
        public BsHealthModel(IBsActorCollection actors, IBsStats stats) : base(actors, Build)
        {
            _stats = stats;
        }

        private static int Build(IBsActor actor)
        {
            return actor.Config().MaxHealth();
        }

        public int Health(IBsActor actor)
        {
            return Value(actor);
        }

        private int Diff(int current, int max, int value)
        {
            var newValue = Math.Min(Math.Max(current + value, 0), max);
            return newValue - current;
        }

        public bool IsDead(IBsActor actor)
        {
            return Value(actor) == 0;
        }

        public void Heal(IBsActor source, IBsActor target, BsActionResult res)
        {
            // Dead characters cannot be healed
            if (IsDead(target))
            {
                res.Reason = "Can't heal a dead character";
                return;
            }

            var current = Value(target);
            var diff = Diff(current, target.Config().MaxHealth(), _stats.ResolveHeal(source, target));
            SetValue(target, current + diff);

            res.Ok = true;
            res.Reason = "Successful Heal";
            res.Value = diff;
        }

        public void Attack(IBsActor source, IBsActor target, BsActionResult res)
        {
            // Dead characters cannot be attacked
            if (IsDead(target))
            {
                res.Reason = "Can't attack a dead character";
                return;
            }

            var current = Value(target);
            var diff = Diff(current, target.Config().MaxHealth(), -_stats.ResolveAttack(source, target));
            SetValue(target, current + diff);

            res.Ok = true;
            res.Reason = "Successful Attack";
            res.Value = diff;
        }
    }
}