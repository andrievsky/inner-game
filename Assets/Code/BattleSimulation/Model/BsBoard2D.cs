using System;
using System.Collections.Generic;
using Code.BattleSimulation.Actor;

namespace Code.BattleSimulation.Model
{
    public interface IBsBoard2D : IBsBoard
    {
        int Height();
        int Width();
    }

    public interface IMover
    {
        bool Move(IBsActor actor, int slotId);
    }

    public class BsBoard2D : IBsBoard2D, IMover
    {
        private struct Slot
        {
            public int X;
            public int Y;
        }

        private readonly int _height;
        private readonly int _width;
        private readonly IBsActor[] _slots;
        private readonly IDictionary<IBsActor, int> _actors;

        public BsBoard2D(int height, int width)
        {
            _height = height;
            _width = width;
            _slots = new IBsActor[width * height];
            _actors = new Dictionary<IBsActor, int>(SlotsCount());
        }

        public int Height()
        {
            return _height;
        }

        public int Width()
        {
            return _width;
        }

        public int SlotsCount()
        {
            return _slots.Length;
        }

        public int FreeSlotsCount()
        {
            return SlotsCount() - _actors.Count;
        }

        public int Distance(IBsActor actor1, IBsActor actor2)
        {
            var slot1 = GetSlot(_actors[actor1]);
            var slot2 = GetSlot(_actors[actor2]);
            return Math.Abs(slot1.X - slot2.X) + Math.Abs(slot1.Y - slot2.Y);
        }

        public IEnumerable<int> FreeSlots()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == null)    
                {
                    yield return i;
                }
            }
        }

        public IEnumerable<int> FreeSlotsInRange(IBsActor actor, int dst)
        {
            var i = _actors[actor];
            var minH = Math.Max(i / _width - dst, 0);
            var maxH = Math.Min(i / _width + dst, _height - 1);
            var minW = Math.Max(i % _width - dst, 0);
            var diffW = Math.Min(i % _width + dst, _width - 1) - minW;
            for (int y = minH; y <= maxH; y++)
            {
                var index = y * _width + minW;
                var max = index + diffW;
                while (index <= max)
                {
                    if (_slots[index] == null)
                    {
                        yield return index;
                    }

                    index++;
                }
            }
        }

        public bool Add(IBsActor actor, int slotId)
        {
            if (_slots[slotId] != null)
            {
                return false;
            }

            if (_actors.ContainsKey(actor))
            {
                return false;
            }

            _slots[slotId] = actor;
            _actors[actor] = slotId;

            return true;
        }

        public IBsActor Actor(int slotId)
        {
            return _slots[slotId];
        }

        public bool Move(IBsActor actor, int slotId)
        {
            _slots[_actors[actor]] = null;
            _actors[actor] = slotId;
            _slots[slotId] = actor;
            return true;
        }

        private int Index(int x, int y)
        {
            return y * _width + x;
        }

        private Slot GetSlot(int slotId)
        {
            return new Slot {X = slotId % _width, Y = slotId / _width};
        }
    }
}