using Code.BattleSimulation;
using Code.BattleSimulation.Actor;
using Moq;
using NUnit.Framework;

namespace Code.Editor.Tests.BattleSimulation
{
    public class BsLevelModelTest
    {
        private Mock<IBsActor> _actor1;
        private Mock<IBsActor> _actor2;
        private Mock<IBsActorCollection> _actors;

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
        }

        [TearDown]
        public void Dispose()
        {
            _actor1 = null;
            _actor2 = null;
            _actors = null;
        }

        [Test]
        public void InitTest()
        {
            var levels = new BsLevels(_actors.Object);
            Assert.AreEqual(2, levels.Data.Count);
            Assert.AreEqual(1, levels.Level(_actor1.Object));
            Assert.AreEqual(1, levels.Level(_actor2.Object));
        }


        [Test]
        public void AddLevelTest()
        {
            var levels = new BsLevels(_actors.Object);
            levels.AddLevel(_actor1.Object, 10);
            Assert.AreEqual(11, levels.Level(_actor1.Object));
            Assert.AreEqual(1, levels.Level(_actor2.Object));
        }

        [Test]
        public void LevelLimitTest()
        {
            var levels = new BsLevels(_actors.Object);
            levels.AddLevel(_actor1.Object, -10);
            Assert.AreEqual(1, levels.Level(_actor1.Object));
            Assert.AreEqual(1, levels.Level(_actor2.Object));
        }
    }
}