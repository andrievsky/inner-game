using System.Linq;
using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Model;
using Moq;
using NUnit.Framework;

namespace Code.Editor.Tests.BattleSimulation
{
    public class BsBoard2DTest
    {
        [Test]
        public void InitBoardTest()
        {
            var board = new BsBoard2D(5, 5);
            Assert.AreEqual(25, board.SlotsCount());
            Assert.AreEqual(25, board.FreeSlotsCount());
            Assert.AreEqual(25, board.FreeSlots().Count());
        }

        [Test]
        public void AddActorTest()
        {
            var board = new BsBoard2D(10, 10);
            var actor1 = new Mock<IBsActor>();
            var actor2 = new Mock<IBsActor>();
            Assert.IsTrue(board.Add(actor1.Object, 0));
            Assert.AreEqual(99, board.FreeSlotsCount());
            Assert.AreEqual(99, board.FreeSlots().Count());
            
            Assert.IsFalse(board.Add(actor2.Object, 0));
            Assert.IsFalse(board.Add(actor1.Object, 0));
            Assert.IsFalse(board.Add(actor1.Object, 1));
            Assert.AreEqual(99, board.FreeSlotsCount());
            Assert.AreEqual(99, board.FreeSlots().Count());
            
            board.Add(actor2.Object, 1);
            Assert.AreEqual(98, board.FreeSlotsCount());
            Assert.AreEqual(98, board.FreeSlots().Count());
        }

        [Test]
        public void FreeSlotsTest()
        {
            var board = new BsBoard2D(2, 2);
            var actor1 = new Mock<IBsActor>();
            board.Add(actor1.Object, 0);

            var free = board.FreeSlots().ToArray();
            Assert.AreEqual(3, free.Length);
            Assert.AreEqual(1, free[0]);
            Assert.AreEqual(2, free[1]);
            Assert.AreEqual(3, free[2]);
            Assert.Contains(1, free);
            Assert.Contains(2, free);
            Assert.Contains(3, free);
        }
        
        [Test]
        public void FreeSlotsInRangeTest()
        {
            var board = new BsBoard2D(10, 10);
            var actor1 = new Mock<IBsActor>();
            var actor2 = new Mock<IBsActor>();
            var actor3 = new Mock<IBsActor>();
            var actor4 = new Mock<IBsActor>();
            board.Add(actor1.Object, 0);
            var free = board.FreeSlotsInRange(actor1.Object, 0).ToArray();
            Assert.AreEqual(0, free.Length);
            
            free = board.FreeSlotsInRange(actor1.Object, 1).ToArray();
            Assert.AreEqual(3, free.Length);
            Assert.Contains(1, free);
            Assert.Contains(10, free);
            Assert.Contains(11, free);
            
            board.Add(actor2.Object, 10);
            free = board.FreeSlotsInRange(actor1.Object, 1).ToArray();
            Assert.AreEqual(2, free.Length);
            Assert.Contains(1, free);
            Assert.Contains(11, free);
            
            free = board.FreeSlotsInRange(actor2.Object, 1).ToArray();
            Assert.AreEqual(4, free.Length);
            Assert.Contains(1, free);
            Assert.Contains(11, free);
            Assert.Contains(20, free);
            Assert.Contains(21, free);
            
            
            board.Add(actor3.Object, 55);
            free = board.FreeSlotsInRange(actor3.Object, 2).ToArray();
            Assert.AreEqual(24, free.Length);
            Assert.Contains(33, free);
            Assert.Contains(34, free);
            Assert.Contains(35, free);
            Assert.Contains(36, free);
            Assert.Contains(37, free);
            Assert.Contains(43, free);
            Assert.Contains(44, free);
            Assert.Contains(45, free);
            Assert.Contains(46, free);
            Assert.Contains(47, free);
            Assert.Contains(53, free);
            Assert.Contains(54, free);
            Assert.Contains(56, free);
            Assert.Contains(57, free);
            Assert.Contains(63, free);
            Assert.Contains(64, free);
            Assert.Contains(65, free);
            Assert.Contains(66, free);
            Assert.Contains(67, free);
            Assert.Contains(73, free);
            Assert.Contains(74, free);
            Assert.Contains(75, free);
            Assert.Contains(76, free);
            Assert.Contains(77, free);
            
            board.Add(actor4.Object, 99);
            free = board.FreeSlotsInRange(actor4.Object, 1).ToArray();
            Assert.AreEqual(3, free.Length);
            Assert.Contains(88, free);
            Assert.Contains(89, free);
            Assert.Contains(98, free);
        }
        
        [Test]
        public void HorDstTest()
        {
            var board = new BsBoard2D(2, 2);
            var actor1 = new Mock<IBsActor>();
            var actor2 = new Mock<IBsActor>();
            board.Add(actor1.Object, 0);
            board.Add(actor2.Object, 1);

            Assert.AreEqual(1, board.Distance(actor1.Object, actor2.Object));
            Assert.AreEqual(1, board.Distance(actor2.Object, actor1.Object));
        }
        
        [Test]
        public void VertDstTest()
        {
            var board = new BsBoard2D(2, 2);
            var actor1 = new Mock<IBsActor>();
            var actor2 = new Mock<IBsActor>();
            board.Add(actor1.Object, 0);
            board.Add(actor2.Object, 2);

            Assert.AreEqual(1, board.Distance(actor1.Object, actor2.Object));
            Assert.AreEqual(1, board.Distance(actor2.Object, actor1.Object));
        }
        
        [Test]
        public void DiagonalDstTest()
        {
            var board = new BsBoard2D(2, 2);
            var actor1 = new Mock<IBsActor>();
            var actor2 = new Mock<IBsActor>();
            board.Add(actor1.Object, 0);
            board.Add(actor2.Object, 3);

            Assert.AreEqual(2, board.Distance(actor1.Object, actor2.Object));
            Assert.AreEqual(2, board.Distance(actor2.Object, actor1.Object));
        }
    }
}