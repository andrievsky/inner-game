using Code.BattleSimulation;
using Code.BattleSimulation.Actor;
using Moq;
using NUnit.Framework;

namespace Code.Editor.Tests.BattleSimulation
{
    public class BsActorCollectionTest
    {
        [Test]
        public void InitTest()
        {
            var actors = new BsActorCollection();
            Assert.AreEqual(0, actors.Count());
        }

        [Test]
        public void AddTest()
        {
            var actors = new BsActorCollection();

            actors.Add(BsTestHelper.DefaultActorConfig);
            Assert.AreEqual(1, actors.Count());
            Assert.AreEqual(0, actors.Actor(0).Id());
            Assert.AreEqual(BsTestHelper.DefaultActorConfig, actors.Actor(0).Config());

            actors.Add(BsTestHelper.DefaultActorConfig);
            Assert.AreEqual(2, actors.Count());
            Assert.AreEqual(0, actors.Actor(0).Id());
            Assert.AreEqual(1, actors.Actor(1).Id());
            Assert.AreEqual(BsTestHelper.DefaultActorConfig, actors.Actor(1).Config());
        }
    }
}