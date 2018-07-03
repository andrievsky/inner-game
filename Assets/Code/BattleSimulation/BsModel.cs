using System;
using System.Collections.Generic;
using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Model;

namespace Code.BattleSimulation
{
    // IBSModel process actions and represents BattleSimulation state 
    public interface IBsModel : IBsHealer, IBsAttacker, IBsFactioner, IBsResultFactory, IMover
    {
        IBsActorCollection Actors();
        IBsBoard2D Board();
        IBsHealth Health();
        IBsLevels Levels();
        IBsFactions Factions();
    }

    // Could be optimized with pool if needed
    public interface IBsResultFactory
    {
        BsActionResult GetResult();
        void ReleaseResult(BsActionResult res);
    }

    public interface IBsSubModel
    {
    }

    public class BsModel : IBsModel
    {
        private readonly IBsActorCollection _actors;
        private IBsBoard2D _board;
        private IBsHealth _health;
        private IBsHealer _healer;
        private IBsAttacker _attacker;
        private IBsLevels _levels;
        private IBsFactions _factions;
        private IBsFactioner _factioner;
        private IBsRange _range;
        private IMover _mover;
        private readonly Stack<BsActionResult> _pool = new Stack<BsActionResult>();

        public BsModel(IBsActorCollection actors)
        {
            if (actors == null)
            {
                throw new ArgumentNullException("actors");
            }

            if (actors.Count() < 2)
            {
                throw new ArgumentException("At least 2 actors expected but was " + _actors.Count());
            }

            _actors = actors;
        }

        public IBsActorCollection Actors()
        {
            return _actors;
        }

        public IBsBoard2D Board()
        {
            return _board;
        }

        public IBsHealth Health()
        {
            return _health;
        }

        public IBsLevels Levels()
        {
            return _levels;
        }

        public IBsFactions Factions()
        {
            return _factions;
        }

        // Makes class construction more simple and flexable
        public void AddSubmodel(IBsSubModel subModel)
        {
            if (subModel is IBsHealth)
            {
                _health = (IBsHealth) subModel;
            }

            if (subModel is IBsHealer)
            {
                _healer = (IBsHealer) subModel;
            }

            if (subModel is IBsAttacker)
            {
                _attacker = (IBsAttacker) subModel;
            }

            if (subModel is IBsBoard2D)
            {
                _board = (IBsBoard2D) subModel;
            }

            if (subModel is IBsLevels)
            {
                _levels = (IBsLevels) subModel;
            }

            if (subModel is IBsFactions)
            {
                _factions = (IBsFactions) subModel;
            }

            if (subModel is IBsFactioner)
            {
                _factioner = (IBsFactioner) subModel;
            }

            if (subModel is IBsRange)
            {
                _range = (IBsRange) subModel;
            }
            
            if (subModel is IMover)
            {
                _mover = (IMover) subModel;
            }
        }

        public void Heal(IBsActor source, IBsActor target, BsActionResult res)
        {
            if(!_factions.AreAllies(source, target))
            {
                res.Reason = "Cann't heal enemies";
                return;
            }
            _healer.Heal(source, target, res);
        }

        public void Attack(IBsActor source, IBsActor target, BsActionResult res)
        {
            if(_factions.AreAllies(source, target))
            {
                res.Reason = "Cann't attack aliies";
                return;
            }
            _range.CheckAttackDst(source, target, res);
            if (!res.Ok)
            {
                return;
            }

            _attacker.Attack(source, target, res);
        }

        public void Join(IBsActor actor, BsFaction faction, BsActionResult res)
        {
            _factioner.Join(actor, faction, res);
        }

        public void Leave(IBsActor actor, BsFaction faction, BsActionResult res)
        {
            _factioner.Leave(actor, faction, res);
        }

        public BsActionResult GetResult()
        {
            if (_pool.Count == 0)
            {
                return new BsActionResult();
            }
            return _pool.Pop();
        }

        public void ReleaseResult(BsActionResult res)
        {
            res.Reset();
            _pool.Push(res);
        }

        public bool Move(IBsActor actor, int slotId)
        {
            return _mover.Move(actor, slotId);
        }
    }
}