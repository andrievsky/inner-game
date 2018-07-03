using System.Linq;
using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Model;
using Moq;
using NUnit.Framework;

namespace Code.Editor.Tests.BattleSimulation
{
    public class BsBoardTest
    {
        [Test]
        public void InitBoardTest()
        {
            var board = new BsBoard(5);
            Assert.AreEqual(5, board.SlotsCount());
            Assert.AreEqual(5, board.FreeSlotsCount());
            Assert.AreEqual(5, board.FreeSlots().Count());
        }

        [Test]
        public void AddActorTest()
        {
            var board = new BsBoard(5);
            var mock = new Mock<IBsActor>();
            board.Add(mock.Object, 0);
            Assert.AreEqual(4, board.FreeSlotsCount());
            var freeSlots = board.FreeSlots();
            Assert.AreEqual(4, freeSlots.Count());
            board.Add(mock.Object, 1);
            board.Add(mock.Object, 2);
            board.Add(mock.Object, 3);
            board.Add(mock.Object, 4);
            Assert.AreEqual(0, board.FreeSlotsCount());
            Assert.AreEqual(0, board.FreeSlots().Count());
        }

        [Test]
        public void FreeSlotsInRangeTest()
        {
            var board = new BsBoard(5);
            var actor = new Mock<IBsActor>();
            board.Add(actor.Object, 0);
            var free = board.FreeSlotsInRange(actor.Object, 1);
            Assert.AreEqual(1, free.Count());

            free = board.FreeSlotsInRange(actor.Object, 10);
            Assert.AreEqual(4, free.Count());
        }
    }
}