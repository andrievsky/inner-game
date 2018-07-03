using System;
using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Config;

namespace Code.BattleSimulation.Model
{
    public interface IBsRange
    {
        void CheckAttackDst(IBsActor source, IBsActor target, BsActionResult res);
    }

    public class BsRange : IBsRange, IBsSubModel
    {
        private readonly IBsBoard _board;
        private readonly IBsDstConfig _config;

        public BsRange(IBsBoard board, IBsDstConfig config)
        {
            _board = board;
            _config = config;
        }

        public void CheckAttackDst(IBsActor source, IBsActor target, BsActionResult res)
        {
            if (_board.Distance(source, target) > ResolveMaxDistance(source))
            {
                res.Reason = "Target out of range";
                return;
            }

            res.Ok = true;
        }

        private int ResolveMaxDistance(IBsActor actor)
        {
            switch (actor.Config().AttackType())
            {
                case ActorAttackType.Melee:
                    return _config.MaxMeleeDistance();
                case ActorAttackType.Range:
                    return _config.MaxRangeDistance();
                default:
                    throw new Exception("AttackType is not supported " + actor.Config().AttackType());
            }
        }
    }
}