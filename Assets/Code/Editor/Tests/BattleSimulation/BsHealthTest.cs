using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Model;
using Moq;
using NUnit.Framework;

namespace Code.Editor.Tests.BattleSimulation
{
    public class BsHealthTest
    {
        private Mock<IBsActor> _actor1;
        private Mock<IBsActor> _actor2;
        private Mock<IBsActorCollection> _actors;
        private Mock<IBsStats> _stats;

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
            _stats = new Mock<IBsStats>();
            _stats.Setup(x => x.ResolveAttack(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(1);
            _stats.Setup(x => x.ResolveHeal(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(1);
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
            var health = new BsHealthModel(_actors.Object, _stats.Object);
            Assert.AreEqual(2, health.Data.Count);
            Assert.AreEqual(1000, health.Health(_actor1.Object));
            Assert.AreEqual(1000, health.Health(_actor2.Object));
        }


        [Test]
        public void AddHealtTest()
        {
            var res = new BsActionResult();
            var health = new BsHealthModel(_actors.Object, _stats.Object);
            health.Attack(_actor1.Object, _actor2.Object, res);
            Assert.AreEqual(1000, health.Health(_actor1.Object));
            Assert.AreEqual(999, health.Health(_actor2.Object));
            Assert.IsTrue(res.Ok);
            Assert.AreEqual(-1, res.Value);

            res = new BsActionResult();
            health.Heal(_actor1.Object, _actor2.Object, res);
            Assert.AreEqual(1000, health.Health(_actor1.Object));
            Assert.AreEqual(1000, health.Health(_actor2.Object));
            Assert.IsTrue(res.Ok);
            Assert.AreEqual(1, res.Value);
        }

        [Test]
        public void HealtLimitTest()
        {
            var res = new BsActionResult();
            _stats.Setup(x => x.ResolveAttack(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(9999);
            _stats.Setup(x => x.ResolveHeal(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(9999);
            var health = new BsHealthModel(_actors.Object, _stats.Object);

            health.Heal(_actor1.Object, _actor2.Object, res);
            Assert.AreEqual(1000, health.Health(_actor2.Object));
            Assert.IsTrue(res.Ok);
            Assert.AreEqual(0, res.Value);

            health.Attack(_actor1.Object, _actor2.Object, res);
            Assert.AreEqual(0, health.Health(_actor2.Object));
            Assert.IsTrue(res.Ok);
            Assert.AreEqual(-1000, res.Value);
        }

        [Test]
        public void IsDeadTest()
        {
            var res = new BsActionResult();
            _stats.Setup(x => x.ResolveAttack(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(9999);
            _stats.Setup(x => x.ResolveHeal(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(9999);

            var health = new BsHealthModel(_actors.Object, _stats.Object);
            Assert.IsFalse(health.IsDead(_actor2.Object));

            health.Attack(_actor1.Object, _actor2.Object, res);
            Assert.IsTrue(health.IsDead(_actor2.Object));
        }

        [Test]
        public void AttackHealTest()
        {
            var res = new BsActionResult();
            _stats.Setup(x => x.ResolveAttack(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(100);
            _stats.Setup(x => x.ResolveHeal(It.IsAny<IBsActor>(), It.IsAny<IBsActor>())).Returns(200);

            var health = new BsHealthModel(_actors.Object, _stats.Object);

            health.Attack(_actor1.Object, _actor2.Object, res);
            Assert.AreEqual(900, health.Health(_actor2.Object));
            Assert.IsTrue(res.Ok);
            Assert.AreEqual(-100, res.Value);

            health.Attack(_actor1.Object, _actor2.Object, res);
            Assert.AreEqual(800, health.Health(_actor2.Object));
            Assert.IsTrue(res.Ok);
            Assert.AreEqual(-100, res.Value);

            health.Attack(_actor1.Object, _actor2.Object, res);
            Assert.AreEqual(700, health.Health(_actor2.Object));
            Assert.IsTrue(res.Ok);
            Assert.AreEqual(-100, res.Value);

            health.Heal(_actor1.Object, _actor2.Object, res);
            Assert.AreEqual(900, health.Health(_actor2.Object));
            Assert.IsTrue(res.Ok);
            Assert.AreEqual(200, res.Value);
        }
    }
}