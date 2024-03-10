using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Tiles.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks
{
    public class TileMatchAppender : IDisposable
    {
        private readonly MatchOrder _matchOrder;
        private readonly TileMatchRule _tileMatchRule;
        private readonly TileMoveController _tileMoveController;

        private List<Vector3> _destinations = new();
        private List<IMatchTile> _matchedTiles = new();
        private List<IMatchTile> _tileOrder = new();

        public bool IsAppended { get; set; }
        public List<Vector3> Destinations => _destinations;
        public List<IMatchTile> MatchedTiles => _matchedTiles;
        public List<IMatchTile> TileOrder => _tileOrder;
        public Transform FreePoint => _matchOrder.FreePoint;

        public TileMatchAppender(MatchOrder matchOrder, TileMatchRule matchRule)
        {
            _matchOrder = matchOrder;
            _tileMatchRule = matchRule;
            _tileMoveController = new(this);
        }

        public async UniTaskVoid Append(IMatchTile tileBlock)
        {
            if (tileBlock.IsLocked)
            {
                IsAppended = false;
                return;
            }

            int matchedIndex;
            Vector3 position;
            int appendIndex = _tileMatchRule.Append(tileBlock.ID);

            if (appendIndex == -1)
            {
                IsAppended = false;
                return;
            }

            IsAppended = true;
            tileBlock.IsSlottedInOrder = true;
            tileBlock.SetColliderEnable(false);

            _tileOrder.Insert(appendIndex, tileBlock);
            position = _matchOrder.Destinations[appendIndex].position;
            _destinations.Insert(appendIndex, position);
            matchedIndex = _tileMatchRule.CheckMatch();

            if(matchedIndex != -1)
            {
                RearrangePosition();
                await UpdateTilePositions();

                TakeMatchedTiles(matchedIndex);
                await ExecuteMatchedTiles();
            }

            RearrangePosition();
            await UpdateTilePositions();
        }

        private void TakeMatchedTiles(int startIndex)
        {
            for (int i = 0; i < MatchConfig.MATCH_RANGE; i++)
            {
                _matchedTiles.Add(_tileOrder[i + startIndex]);
            }

            _tileOrder.RemoveRange(startIndex, MatchConfig.MATCH_RANGE);
            _destinations.RemoveRange(startIndex, MatchConfig.MATCH_RANGE);
        }

        private void RearrangePosition()
        {
            if (_destinations.Count > _matchOrder.Destinations.Length)
                return;

            for (int i = 0; i < _tileOrder.Count; i++)
            {
                _destinations[i] = _matchOrder.Destinations[i].position;
            }
        }

        private UniTask ExecuteMatchedTiles()
        {
            return _tileMoveController.ExecuteMatchedTiles();
        }

        private UniTask UpdateTilePositions()
        {
            return _tileMoveController.UpdateTilePositions();
        }

        public void Dispose()
        {
            _tileMatchRule.Dispose();
            _destinations.Clear();
            _matchedTiles.Clear();
            _tileOrder.Clear();
        }
    }
}
