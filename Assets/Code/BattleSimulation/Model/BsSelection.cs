using Code.BattleSimulation.Actor;

namespace Code.BattleSimulation.Model
{
    public interface IBsSelection : IBsSubModel
    {
        void Select(IBsActor actor);
        IBsActor SelectedActor();
    }

    public class BsSelection : IBsSelection
    {
        private IBsActor _selectedActor;

        public void Select(IBsActor actor)
        {
            _selectedActor = actor;
        }

        public IBsActor SelectedActor()
        {
            return _selectedActor;
        }
    }
}