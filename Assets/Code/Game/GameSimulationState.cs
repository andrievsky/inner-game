using Code.BattleSimulation;
using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Model;
using Code.BattleSimulation.View;
using Code.Common.StateMachine;
using Code.Input;
using Code.Logger;

namespace Code.Game
{
    public class GameSimulationState : AbstractState
    {
        private readonly ILog _logger;
        private readonly IBsView _view;
        private readonly IBsHudView _hudView;
        private IBsModel _model;
        private IBsPresenter _presenter;
        private readonly IInputController _input;
        private readonly GameConfig _config;

        public GameSimulationState(ILog logger, IBsView view, IBsHudView hudView, IInputController input, GameConfig config)
        {
            _logger = logger;
            _view = view;
            _hudView = hudView;
            _input = input;
            _config = config;
        }

        public override void DidEnter(IState state)
        {
            // Setup the board
            var board = new BsBoard2D(10, 10);

            // Create actors and add them to the board
            var actors = new BsActorCollection();
            var red1 = actors.Add(_config.Fighter);
            var red2 = actors.Add(_config.Fighter);
            var red3 = actors.Add(_config.Fighter);
            var blue1 = actors.Add(_config.Archer);
            var blue2 = actors.Add(_config.Fighter);
            var blue3 = actors.Add(_config.Archer);
            
            board.Add(red1, 0);
            board.Add(red2, 5);
            board.Add(red3, 9);
            board.Add(blue1, 90);
            board.Add(blue2, 95);
            board.Add(blue3, 99);
            

            // Setup models
            var levels = new BsLevels(actors);
            var stats = new BsStatsModel(levels);
            var health = new BsHealthModel(actors, stats);
            var range = new BsRange(board, _config.DstConfig);
            var factions = new BsFactions(actors);
            
            // Add actors to factions
            factions.Add(red1, BsFaction.Red);
            factions.Add(red2, BsFaction.Red);
            factions.Add(red3, BsFaction.Red);
            factions.Add(blue1, BsFaction.Blue);
            factions.Add(blue2, BsFaction.Blue);
            factions.Add(blue3, BsFaction.Blue);

            // Setup levels
            levels.AddLevel(actors.Actor(1), 5);
            levels.AddLevel(actors.Actor(2), 10);
            levels.AddLevel(actors.Actor(3), 15);
            levels.AddLevel(actors.Actor(4), 20);

            // Facade model
            var model = new BsModel(actors);
            model.AddSubmodel(health);
            model.AddSubmodel(board);
            model.AddSubmodel(levels);
            model.AddSubmodel(range);
            model.AddSubmodel(factions);
            _model = model;
            
            // Presenter
            _presenter = new BsPresenter(_view, _hudView, _model, _input, _logger);
            _presenter.Init();

            _logger.Log("Battle Simulation Ready");
        }

        public override void Update(int deltaTime)
        {
        }

        public override void WillExit(IState state)
        {
        }
    }
}