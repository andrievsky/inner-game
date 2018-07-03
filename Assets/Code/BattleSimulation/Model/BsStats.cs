using System;
using Code.BattleSimulation.Actor;

namespace Code.BattleSimulation.Model
{
    public interface IBsStats : IBsSubModel
    {
        int ResolveHeal(IBsActor source, IBsActor target);
        int ResolveAttack(IBsActor source, IBsActor target);
    }

    public class BsStatsModel : IBsStats
    {
        private readonly IBsLevels _levels;

        public BsStatsModel(IBsLevels levels)
        {
            _levels = levels;
        }

        public int ResolveHeal(IBsActor source, IBsActor target)
        {
            return source.Config().BaseHealing();
        }

        public int ResolveAttack(IBsActor source, IBsActor target)
        {
            return ProcessAttack(source.Config().BaseDamage(), _levels.Level(source) - _levels.Level(target));
        }

        private static int ProcessAttack(int value, int diff)
        {
            if (value == 0 || Math.Abs(value) == 1)
            {
                return value;
            }

            if (diff <= -5)
            {
                return value / 2;
            }

            if (diff >= 5)
            {
                return (int) Math.Round(value * 1.5d);
            }

            return value;
        }
    }
}