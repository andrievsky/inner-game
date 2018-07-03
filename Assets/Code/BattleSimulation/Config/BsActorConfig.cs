using System;

namespace Code.BattleSimulation.Config
{
    public enum ActorAttackType
    {
        Melee,
        Range
    }
    
    public interface IBsActorConfig
    {
        int MaxHealth();
        int BaseDamage();
        int BaseHealing();
        int MoveDst();
        ActorAttackType AttackType();
    }

    [Serializable]
    public class BsActorConfig : IBsActorConfig
    {
        public int MaxHealthValue = 1000;
        public int BaseDamageValue = 1;
        public int BaseHealingValue = 1;
        public int MoveDstValue = 3;
        public ActorAttackType AttackTypeValue = ActorAttackType.Melee;

        public int MaxHealth()
        {
            return MaxHealthValue;
        }

        public int BaseDamage()
        {
            return BaseDamageValue;
        }

        public int BaseHealing()
        {
            return BaseHealingValue;
        }

        public ActorAttackType AttackType()
        {
            return AttackTypeValue;
        }

        public int MoveDst()
        {
            return MoveDstValue;
        }
    }
}