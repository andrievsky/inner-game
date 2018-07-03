using Code.BattleSimulation.Config;

namespace Code.BattleSimulation.Actor
{
    // IBSActor used as a atomic parameter for BattleSimulation entities
    public interface IBsActor
    {
        int Id();
        IBsActorConfig Config();
    }

    public class BsActor : IBsActor
    {
        private readonly int _id;
        private readonly IBsActorConfig _config;

        public BsActor(int id, IBsActorConfig config)
        {
            _id = id;
            _config = config;
        }

        public int Id()
        {
            return _id;
        }

        public IBsActorConfig Config()
        {
            return _config;
        }
    }
}