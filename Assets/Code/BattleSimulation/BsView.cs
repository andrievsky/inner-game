using System;
using System.Collections.Generic;
using Code.BattleSimulation.Actor;
using Code.BattleSimulation.Model;
using Code.Extensions;
using UnityEngine;
using Zenject;

namespace Code.BattleSimulation
{
    public interface IBsView
    {
        event Action<IBsActor> OnActorClick;
        event Action<int> OnSlotClick;

        void SetBoard(IBsBoard2D board);

        void Move(IBsActor actor, int slotId);

        void SetClickableSlots(IList<int> slots);

        void ClearClickableSlots();

        // To simplify Presenter
        void ClearActions();
    }

    // Refactor this view, should be clear and simple
    public class BsView : MonoInstaller<BsView>, IBsView
    {
        class ActorsData
        {
            public SpriteRenderer View;
            public int SlotId;
        }

        public override void InstallBindings()
        {
            Container.Bind<IBsView>().FromInstance(this).AsSingle();
        }

        [SerializeField] public Transform TilesContainer;
        [SerializeField] public Transform ActorsContainer;
        [SerializeField] public SpriteRenderer ActorPref;
        [SerializeField] public SpriteRenderer TilePref;
        [SerializeField] public Sprite NormalTileSprite;
        [SerializeField] public Sprite ClickableTileSprite;

        public event Action<IBsActor> OnActorClick;
        public event Action<int> OnSlotClick;

        private static readonly IList<int> EmptySlots = new int[] { };

        private SpriteRenderer _lastSelectedActorView;
        private IList<int> _clickableSlots = EmptySlots;

        private readonly IDictionary<IBsActor, ActorsData> _actors = new Dictionary<IBsActor, ActorsData>();

        private readonly IList<SpriteRenderer> _tileViews = new List<SpriteRenderer>();

        // Should be removed from View
        private IBsBoard2D _board;

        public void SetBoard(IBsBoard2D board)
        {
            Clear();
            _actors.Clear();
            _tileViews.Clear();
            _actors.Clear();
            _board = board;
            if (board.Width() == 0)
            {
                throw new Exception("Board width should be greater than 0");
            }

            var slotId = 0;
            for (int y = 0; y < board.Height(); y++)
            {
                for (int x = 0; x < board.Width(); x++)
                {
                    var view = Instantiate<SpriteRenderer>(TilePref, new Vector3(x, y, 0), Quaternion.identity);
                    view.transform.SetParent(TilesContainer, false);
                    _tileViews.Add(view);

                    var actor = board.Actor(slotId);
                    if (actor != null)
                    {
                        var actorView = Instantiate<SpriteRenderer>(ActorPref, ActorsContainer);
                        actorView.transform.position = view.transform.position;
                        _actors[actor] = new ActorsData {View = actorView, SlotId = slotId};
                    }

                    slotId++;
                }
            }
        }

        public void Move(IBsActor actor, int slotId)
        {
            _actors[actor].SlotId = slotId;
            _actors[actor].View.transform.position = _tileViews[slotId].transform.position;
        }

        public void SetClickableSlots(IList<int> slots)
        {
            foreach (var slotId in _clickableSlots)
            {
                _tileViews[slotId].sprite = NormalTileSprite;
            }

            _clickableSlots = slots;
            foreach (var slotId in _clickableSlots)
            {
                _tileViews[slotId].sprite = ClickableTileSprite;
            }
        }

        public void ClearClickableSlots()
        {
            SetClickableSlots(EmptySlots);
        }

        private void Clear()
        {
            var cnt = ActorsContainer.childCount;
            for (int i = cnt - 1; i >= 0; i--)
            {
                Destroy(ActorsContainer.GetChild(i));
            }

            ClearActions();
        }

        public void ClearActions()
        {
            OnActorClick = null;
        }

        private void OnMouseDown()
        {
            var p = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            var slotId = (int) p.y * _board.Width() + (int) p.x;

            foreach (var actor in _actors.Values)
            {
                if (actor.SlotId != slotId)
                {
                    continue;
                }

                OnActorClick.Dispatch(_board.Actor(slotId));
                return;
            }

            foreach (var i in _clickableSlots)
            {
                if (slotId != i)
                {
                    continue;
                }

                OnSlotClick.Dispatch(slotId);
                return;
            }
        }
    }
}