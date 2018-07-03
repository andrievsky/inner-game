using System;

namespace Code.BattleSimulation.Config
{
    public interface IBsDstConfig
    {
        int MaxMeleeDistance();
        int MaxRangeDistance();
    }
    
    [Serializable]
    public class BsDstConfig : IBsDstConfig
    {
        public int MaxMeleeDistanceValue;
        public int MaxRangeDistanceValue;

        public int MaxMeleeDistance()
        {
            return MaxMeleeDistanceValue;
        }

        public int MaxRangeDistance()
        {
            return MaxRangeDistanceValue;
        }
    }
}