using Code.BattleSimulation.Config;
using Zenject;

namespace Code.Game
{
    public class GameConfig : ScriptableObjectInstaller<GameConfig>
    {
        public BsActorConfig Fighter;
        public BsActorConfig Archer;
        public BsDstConfig DstConfig;

        public override void InstallBindings()
        {
            Container.BindInstances(this);
        }
    }
}