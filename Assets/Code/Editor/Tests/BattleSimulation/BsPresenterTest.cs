using Code.BattleSimulation;
using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Model;
using Code.BattleSimulation.View;
using Code.Input;
using Code.Logger;
using Moq;
using NUnit.Framework;

namespace Code.Editor.Tests.BattleSimulation
{
    public class BsPresenterTest
    {
        private Mock<IBsActor> _actor1;
        private Mock<IBsActor> _actor2;
        private Mock<IBsActorCollection> _actors;
        private Mock<IBsView> _view;
        private Mock<IBsHudView> _hudView;
        private Mock<IBsModel> _model;
        private Mock<IInputController> _input;
        private Mock<IBsHealth> _health;
        private Mock<IBsLevels> _levels;
        private Mock<ILog> _logger;
        private Mock<IBsBoard2D> _board;
        private Mock<IBsFactions> _factions;

        [SetUp]
        public void Init()
        {
            _actor1 = new Mock<IBsActor>();
            _actor1.Setup(x => x.Id()).Returns(0);
            _actor1.Setup(x => x.Config()).Returns(BsTestHelper.DefaultActorConfig);
            _actor2 = new Mock<IBsActor>();
            _actor2.Setup(x => x.Id()).Returns(1);
            _actor2.Setup(x => x.Config()).Returns(BsTestHelper.DefaultActorConfig);
            _actors = new Mock<IBsActorCollection>();
            _actors.Setup(x => x.Count()).Returns(2);
            _actors.Setup(x => x.Actor(0)).Returns(_actor1.Object);
            _actors.Setup(x => x.Actor(1)).Returns(_actor2.Object);
            _health = new Mock<IBsHealth>();
            _levels = new Mock<IBsLevels>();
            _view = new Mock<IBsView>();
            _board = new Mock<IBsBoard2D>();
            _factions = new Mock<IBsFactions>();
            _model = new Mock<IBsModel>();
            _model.Setup(x => x.Actors()).Returns(_actors.Object);
            _model.Setup(x => x.Health()).Returns(_health.Object);
            _model.Setup(x => x.Levels()).Returns(_levels.Object);
            _model.Setup(x => x.Board()).Returns(_board.Object);
            _model.Setup(x => x.Factions()).Returns(_factions.Object);
            _input = new Mock<IInputController>();
            _logger = new Mock<ILog>();
            _hudView = new Mock<IBsHudView>();
        }

        [TearDown]
        public void Dispose()
        {
            _actor1 = null;
            _actor2 = null;
            _actors = null;
            _health = null;
            _levels = null;
            _view = null;
            _model = null;
            _input = null;
            _logger = null;
            _board = null;
            _hudView = null;
        }

        [Test]
        public void InitTest()
        {
            var presenter = new BsPresenter(_view.Object, _hudView.Object, _model.Object, _input.Object, _logger.Object);

            _model.Verify(x => x.Actors(), Times.Never());
            _input.Verify(x => x.SetBinds(It.IsAny<IKeyBindMap>()), Times.Never());

            presenter.Init();
            _model.Verify(x => x.Actors(), Times.AtLeastOnce());
            _input.Verify(x => x.SetBinds(It.IsAny<IKeyBindMap>()), Times.Once());
        }
    }
}