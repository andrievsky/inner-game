using System;
using System.Linq;
using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Model;
using Code.BattleSimulation.View;
using Code.Input;
using Code.Logger;
using UnityEngine;

namespace Code.BattleSimulation
{
    // IBSPresenter represents BS Controller lifecycle 
    public interface IBsPresenter : IDisposable
    {
        void Init();
    }

    // Getting input from InputController and View, apply Actions, updates Model and View
    public class BsPresenter : IBsPresenter
    {
        private readonly IBsView _view;
        private readonly IBsHudView _hudView;
        private readonly IBsModel _model;
        private readonly IInputController _input;
        private readonly ILog _logger;
        private readonly IBsSelection _selection;

        public BsPresenter(IBsView view, IBsHudView hudView, IBsModel model, IInputController input, ILog logger)
        {
            _view = view;
            _hudView = hudView;
            _model = model;
            _input = input;
            _logger = logger;
            _selection = new BsSelection();
        }

        public void Init()
        {
            if (_model.Actors().Count() == 0)
            {
                throw new Exception("At lease 1 actor expected from IBSModel.Actors() but was 0");
            }

            _view.SetBoard(_model.Board());

            // Not a perfect place for keybindings, should be moved to another level of abstraction if the game will get more Presenters
            var map = new KeyBindMap();
            map.Add(KeyCode.Q, OnSelectKey);
            map.Add(KeyCode.W, OnAttackKey);
            map.Add(KeyCode.E, OnHealKey);
            map.Add(KeyCode.Alpha1, () => { JoinOrLeaveFraction(BsFaction.Red); });
            map.Add(KeyCode.Alpha2, () => { JoinOrLeaveFraction(BsFaction.Green); });
            map.Add(KeyCode.Alpha3, () => { JoinOrLeaveFraction(BsFaction.Blue); });

            _input.SetBinds(map);
            OnActorSelect(_model.Actors().Actor(0));
            OnSelectKey();
            
            _view.OnSlotClick += OnOnSlotClick;
        }

        private void OnOnSlotClick(int slotId)
        {
            _view.ClearClickableSlots();
            if (_model.Move(_selection.SelectedActor(), slotId))
            {
                _view.Move(_selection.SelectedActor(), slotId);
            }
        }

        private void OnSelectKey()
        {
            _view.ClearActions();
            _view.OnActorClick += OnActorSelect;
        }

        private void OnAttackKey()
        {
            _view.ClearActions();
            _view.OnActorClick += OnActorAttack;
        }

        private void OnHealKey()
        {
            _view.ClearActions();
            _view.OnActorClick += OnActorHeal;
        }

        private void OnActorSelect(IBsActor actor)
        {
            _selection.Select(actor);
            _view.SetClickableSlots(_model.Board().FreeSlotsInRange(actor, actor.Config().MoveDst()).ToArray());
            UpdateActor(actor);
        }

        private void OnActorAttack(IBsActor actor)
        {
            var res = _model.GetResult();
            _model.Attack(_selection.SelectedActor(), actor, res);
            UpdateLog(res);
            UpdateActor(actor);
            _model.ReleaseResult(res);
        }

        private void OnActorHeal(IBsActor actor)
        {
            var res = _model.GetResult();
            _model.Heal(_selection.SelectedActor(), actor, res);
            UpdateLog(res);
            UpdateActor(actor);
            _model.ReleaseResult(res);
        }

        private void JoinOrLeaveFraction(BsFaction faction)
        {
            var res = _model.GetResult();
            var actor = _selection.SelectedActor();
            if (_model.Factions().Contains(actor, faction))
            {
                _model.Leave(actor, faction, res);
            }
            else
            {
                _model.Join(actor, faction, res);
            }

            UpdateLog(res);
            UpdateActor(actor);
            _model.ReleaseResult(res);
        }

        public void Dispose()
        {
            _view.ClearActions();
            _input.ClearBinds();
        }

        private void UpdateLog(BsActionResult result)
        {
            _logger.Log(result.Reason);
        }

        private void UpdateActor(IBsActor actor)
        {
            _hudView.SetDetails(actor, _model.Health().Health(actor), _model.Health().IsDead(actor),
                _model.Levels().Level(actor), actor.Config().AttackType(), _model.Factions().Factions(actor));
        }
    }
}