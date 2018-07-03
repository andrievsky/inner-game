using Code.BattleSimulation.Config;
using Moq;

namespace Code.Editor.Tests.BattleSimulation
{
    public class BsTestHelper
    {
        public static readonly IBsActorConfig DefaultActorConfig = MockConfig();

        private static IBsActorConfig MockConfig()
        {
            var config = new Mock<IBsActorConfig>();
            config.Setup(x => x.MaxHealth()).Returns(1000);
            config.Setup(x => x.BaseDamage()).Returns(1);
            config.Setup(x => x.BaseHealing()).Returns(1);
            config.Setup(x => x.AttackType()).Returns(ActorAttackType.Melee);
            return config.Object;
        }
    }
}