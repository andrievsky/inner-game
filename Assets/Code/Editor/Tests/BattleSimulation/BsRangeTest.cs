using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Config;
using Code.BattleSimulation.Model;
using Moq;
using NUnit.Framework;

namespace Code.Editor.Tests.BattleSimulation
{
    public class BsRangeTest
    {
        private Mock<IBsActor> _actor1;
        private Mock<IBsActor> _actor2;
        private Mock<IBsDstConfig> _config;
        private Mock<IBsBoard> _board;

        [SetUp]
        public void Init()
        {
            _actor1 = new Mock<IBsActor>();
            _actor1.Setup(x => x.Config()).Returns(BsTestHelper.DefaultActorConfig);
            _actor2 = new Mock<IBsActor>();
            _actor2.Setup(x => x.Config()).Returns(BsTestHelper.DefaultActorConfig);
            _config = new Mock<IBsDstConfig>();
            _config.Setup(x => x.MaxMeleeDistance()).Returns(2);
            _config.Setup(x => x.MaxRangeDistance()).Returns(20);
            _board = new Mock<IBsBoard>();
        }

        [TearDown]
        public void Dispose()
        {
            _actor1 = null;
            _actor2 = null;
            _config = null;
            _board = null;
        }

        [Test]
        public void MeleeInRangeTest()
        {
            var model = new BsRange(_board.Object, _config.Object);

            for (int i = 0; i <= 2; i++)
            {
                var res = new BsActionResult();
                _board.Setup(x => x.Distance(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(i);
                model.CheckAttackDst(_actor1.Object, _actor2.Object, res);
                Assert.IsTrue(res.Ok);
            }

            for (int i = 3; i < 25; i = i << 1)
            {
                var res = new BsActionResult();
                _board.Setup(x => x.Distance(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(i);
                model.CheckAttackDst(_actor1.Object, _actor2.Object, res);
                Assert.IsFalse(res.Ok);
            }
        }

        [Test]
        public void RangedInRangeTest()
        {
            var rangeActorConfig = new Mock<IBsActorConfig>();
            rangeActorConfig.Setup(x => x.AttackType()).Returns(ActorAttackType.Range);

            _actor1.Setup(x => x.Config()).Returns(rangeActorConfig.Object);
            var model = new BsRange(_board.Object, _config.Object);

            for (int i = 1; i <= 20; i = i << 1)
            {
                var res = new BsActionResult();
                _board.Setup(x => x.Distance(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(i);
                model.CheckAttackDst(_actor1.Object, _actor2.Object, res);
                Assert.IsTrue(res.Ok);
            }

            for (int i = 21; i < 100; i = i << 1)
            {
                var res = new BsActionResult();
                _board.Setup(x => x.Distance(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(i);
                model.CheckAttackDst(_actor1.Object, _actor2.Object, res);
                Assert.IsFalse(res.Ok);
            }
        }
    }
}