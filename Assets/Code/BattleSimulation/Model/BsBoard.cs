using System;
using System.Collections.Generic;
using Code.BattleSimulation.Actor;

namespace Code.BattleSimulation.Model
{
    // BSBoard represents a simulation space with on limited amount of abstract spots
    // One spot per Actor
    public interface IBsBoard : IBsSubModel
    {
        int SlotsCount();
        int FreeSlotsCount();
        IBsActor Actor(int slotId);
        IEnumerable<int> FreeSlots();
        IEnumerable<int> FreeSlotsInRange(IBsActor actor, int dst);
        int Distance(IBsActor actor1, IBsActor actor2);
    }

    public class BsBoard : IBsBoard
    {
        private readonly IBsActor[] _slots;
        private int _actorsCount;

        public BsBoard(int slotsCount)
        {
            if (slotsCount < 1)
            {
                throw new ArgumentException("Slots count should be more than 0");
            }

            _slots = new IBsActor[slotsCount];
        }

        public int SlotsCount()
        {
            return _slots.Length;
        }

        public int FreeSlotsCount()
        {
            return _slots.Length - _actorsCount;
        }

        public IEnumerable<int> FreeSlots()
        {
            for (var j = 0; j < _slots.Length; j++)
            {
                if (_slots[j] != null)
                {
                    continue;
                }

                yield return j;
            }
        }

        public IEnumerable<int> FreeSlotsInRange(IBsActor actor, int dst)
        {
            var slotId = FindSlot(actor);

            for (var j = Math.Max(slotId - dst, 0); j < Math.Min(slotId + dst + 1, _slots.Length); j++)
            {
                if (_slots[j] != null)
                {
                    continue;
                }

                yield return j;
            }
        }

        public IBsActor Actor(int slotId)
        {
            return _slots[slotId];
        }

        public bool Add(IBsActor actor, int slotId)
        {
            if (FreeSlotsCount() == 0)
            {
                return false;
            }

            if (slotId < 0 || slotId >= _slots.Length)
            {
                return false;
            }

            if (_slots[slotId] != null)
            {
                return false;
            }

            _slots[slotId] = actor;
            _actorsCount++;
            return true;
        }

        public int Distance(IBsActor actor1, IBsActor actor2)
        {
            var slot1 = FindSlot(actor1);
            var slot2 = FindSlot(actor2);
            return Math.Abs(slot1 - slot2);
        }

        // Can be optimized
        private int FindSlot(IBsActor actor)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == actor)
                {
                    return i;
                }
            }

            throw new Exception("Can not find a slot for Actor " + actor.Id());
        }
    }
}