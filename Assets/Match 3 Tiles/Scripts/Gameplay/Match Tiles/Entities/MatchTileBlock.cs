using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Match3Tiles.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine.Rendering;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles.Entities
{
    public class MatchTileBlock : MonoBehaviour, IObserver<TileData>, IMatchTile, IMatchTileMove
    {
        [SerializeField] private int id;
        [SerializeField] private BlockTileAnimation tileAnimation;

        [Space(10)]
        [SerializeField] private SortingGroup tileGroup;
        [SerializeField] private SpriteRenderer tileImage;
        [SerializeField] private SpriteRenderer background;

        [Space(10)]
        [SerializeField] private BoxCollider2D tileCollider;
        [SerializeField] private LayerMask tileLayer;

        [Space(10)]
        [SerializeField] private Color enableColor;
        [SerializeField] private Color disableColor;

        private bool _isLocked;
        private int _overlappedCount = 0;

        private TileData _tileData;
        private IDisposable _unsubscriber;

        private ContactFilter2D _contactFilter;
        private Collider2D[] _tileColliders = new Collider2D[20];
        private List<IMatchTile> _overlappedTiles = new();

        public bool IsLocked => _isLocked;

        public int ID => id;
        public int Priority => tileGroup != null ? tileGroup.sortingOrder : 0;
        public bool IsSlottedInOrder { get; set; }

        public TileData TileData => _tileData;

        public Vector3 Position => transform.position;

        private void Awake()
        {
            _contactFilter = new ContactFilter2D
            {
                layerMask = tileLayer,
                minDepth = -10,
                maxDepth = 10,
                useTriggers = true
            };
        }

        public void SetTileData(TileData tileData)
        {
            _tileData = tileData;
            id = _tileData.ID;
            tileImage.sprite = _tileData.Icon;
        }

        public void SetColliderEnable(bool enable)
        {
            tileCollider.enabled = enable;
        }

        public UniTask MoveTo(Vector3 position)
        {
            return tileAnimation.MoveTo(position);
        }

        public UniTask ReturnToOriginalPosition()
        {
            return MoveTo(_tileData.Position);
        }

        public void CheckOverlap()
        {
            _overlappedCount = Physics2D.OverlapBox(transform.position, tileCollider.size, 0, _contactFilter, _tileColliders);
            
            for (int i = 0; i < _overlappedCount; i++)
            {
                if (_tileColliders[i].TryGetComponent(out IMatchTile tileBlock))
                {
                    if (tileBlock.Priority > this.Priority && tileBlock.ID != id)
                    {
                        _overlappedTiles.Add(tileBlock);
                    }
                }
            }
        }

        public void CheckTileUnlock()
        {
            _isLocked = false;

            for (int i = 0; i < _overlappedTiles.Count; i++)
            {
                if (!_overlappedTiles[i].IsSlottedInOrder)
                {
                    _isLocked = true;
                    break;
                }
            }

            SetColliderEnable(!_isLocked);
            SetBackgroundColor(_isLocked);
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(TileData value) { }

        public void Subscribe(IObservable<TileData> observable)
        {
            _unsubscriber = observable.Subscribe(this);
        }

        public void SetBackgroundColor(bool isLocked)
        {
            background.color = isLocked ? disableColor : enableColor;
        }

        public void SetSortingOrder(int sortingOrder)
        {
            tileGroup.sortingOrder = sortingOrder;
        }

        public void Clear()
        {
            _overlappedTiles.Clear();
            SimplePool.Despawn(this.gameObject);
        }

        private void OnDestroy()
        {
            _unsubscriber?.Dispose();
        }
    }
}
