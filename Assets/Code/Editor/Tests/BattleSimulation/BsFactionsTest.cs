using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Model;
using Moq;
using NUnit.Framework;

namespace Code.Editor.Tests.BattleSimulation
{
    public class BsFactionsTest
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
            var model = new BsFactions(_actors.Object);
            Assert.IsFalse(model.AreAllies(_actor1.Object, _actor2.Object));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Red));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Green));
            Assert.IsFalse(model.Contains(_actor2.Object, BsFaction.Blue));
            Assert.IsFalse(model.Contains(_actor2.Object, BsFaction.Red));
            Assert.IsFalse(model.Contains(_actor2.Object, BsFaction.Green));
        }

        [Test]
        public void AddOneFactionTest()
        {
            var model = new BsFactions(_actors.Object);
            model.Add(_actor1.Object, BsFaction.Blue);
            Assert.IsFalse(model.AreAllies(_actor1.Object, _actor2.Object));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Red));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Green));

            model.Add(_actor1.Object, BsFaction.Blue);
            Assert.IsFalse(model.AreAllies(_actor1.Object, _actor2.Object));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Red));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Green));
        }

        [Test]
        public void AddAllFactionsTest()
        {
            var model = new BsFactions(_actors.Object);
            model.Add(_actor1.Object, BsFaction.Blue);
            model.Add(_actor1.Object, BsFaction.Red);
            model.Add(_actor1.Object, BsFaction.Green);
            Assert.IsFalse(model.AreAllies(_actor1.Object, _actor2.Object));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Red));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Green));
        }

        [Test]
        public void AddGroupTest()
        {
            var model = new BsFactions(_actors.Object);
            model.Add(_actor1.Object, BsFaction.Blue | BsFaction.Green);
            Assert.IsFalse(model.AreAllies(_actor1.Object, _actor2.Object));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Red));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Green));
        }


        [Test]
        public void RemoveTest()
        {
            var model = new BsFactions(_actors.Object);
            model.Add(_actor1.Object, BsFaction.Blue);
            model.Add(_actor1.Object, BsFaction.Red);
            model.Add(_actor1.Object, BsFaction.Green);

            model.Remove(_actor1.Object, BsFaction.Blue);
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Red));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Green));

            model.Remove(_actor1.Object, BsFaction.Blue);
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Red));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Green));

            model.Remove(_actor1.Object, BsFaction.Red);
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Red));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Green));

            model.Remove(_actor1.Object, BsFaction.Green);
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Red));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Green));
        }

        [Test]
        public void RemoveGroupTest()
        {
            var model = new BsFactions(_actors.Object);
            model.Add(_actor1.Object, BsFaction.Blue);
            model.Add(_actor1.Object, BsFaction.Red);
            model.Add(_actor1.Object, BsFaction.Green);

            model.Remove(_actor1.Object, BsFaction.Blue | BsFaction.Green);
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Red));
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Green));
        }

        [Test]
        public void AreAlliesTest()
        {
            var model = new BsFactions(_actors.Object);
            model.Add(_actor1.Object, BsFaction.Blue);
            model.Add(_actor2.Object, BsFaction.Blue);
            Assert.IsTrue(model.AreAllies(_actor1.Object, _actor2.Object));

            model.Remove(_actor2.Object, BsFaction.Blue);
            Assert.IsFalse(model.AreAllies(_actor1.Object, _actor2.Object));
        }

        [Test]
        public void AreAlliesGroupsTest()
        {
            var model = new BsFactions(_actors.Object);
            model.Add(_actor1.Object, BsFaction.Green | BsFaction.Blue);
            model.Add(_actor2.Object, BsFaction.Red | BsFaction.Blue);
            Assert.IsTrue(model.AreAllies(_actor1.Object, _actor2.Object));
        }

        [Test]
        public void FactionsTest()
        {
            var model = new BsFactions(_actors.Object);
            model.Add(_actor1.Object, BsFaction.Green | BsFaction.Blue);
            model.Factions(_actor2.Object);
            Assert.AreEqual("Green, Blue", model.Factions(_actor1.Object).ToString());
        }

        [Test]
        public void JoinFactionTest()
        {
            var model = new BsFactions(_actors.Object);
            var res = new BsActionResult();

            model.Join(_actor1.Object, BsFaction.Blue, res);
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsTrue(res.Ok);

            res = new BsActionResult();
            model.Join(_actor1.Object, BsFaction.Blue, res);
            Assert.IsTrue(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsFalse(res.Ok);
        }

        [Test]
        public void LeaveFactionTest()
        {
            var model = new BsFactions(_actors.Object);
            var res = new BsActionResult();

            model.Join(_actor1.Object, BsFaction.Blue, res);

            res = new BsActionResult();
            model.Leave(_actor1.Object, BsFaction.Blue, res);
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsTrue(res.Ok);

            res = new BsActionResult();
            model.Leave(_actor1.Object, BsFaction.Blue, res);
            Assert.IsFalse(model.Contains(_actor1.Object, BsFaction.Blue));
            Assert.IsFalse(res.Ok);
        }
    }
}