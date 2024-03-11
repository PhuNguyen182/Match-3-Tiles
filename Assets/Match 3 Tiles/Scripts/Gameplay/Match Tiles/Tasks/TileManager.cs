using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Tiles.Scripts.Gameplay.Factories;
using Match3Tiles.Scripts.GameData.LevelData.CustomData;
using Match3Tiles.Scripts.Databases;
using Match3Tiles.Scripts.Utils;
using Match3Tiles.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks
{
    public class TileManager : IDisposable
    {
        private readonly GameInput _gameInput;
        private readonly TileMatchRule _tileMatchRule;
        private readonly TileSpriteDatabase _spriteDatabase;
        private readonly MatchTileFactory _matchTileFactory;
        private readonly CancellationTokenSource _tokenSource;

        private int _totalTileCount;
        private CancellationToken _token;
        private List<IMatchTile> _matchTiles;

        public Action OnLevelWin;
        public Action OnLevelLose;

        public TileManager(MatchTileFactory tileFactory, TileMatchRule tileMatchRule
                          , TileSpriteDatabase spriteDatabase, GameInput gameInput)
        {
            _matchTiles = new();
            _tileMatchRule = tileMatchRule;
            _spriteDatabase = spriteDatabase;
            _matchTileFactory = tileFactory;
            _gameInput = gameInput;

            _tokenSource = new();
            _token = _tokenSource.Token;
        }

        // This function is used only for importing level from data
        public void GererateTilesToImport(List<BlockTileData> blockTileDatas)
        {
            for (int i = 0; i < blockTileDatas.Count; i++)
            {
                TileData tileData = new TileData
                {
                    ID = blockTileDatas[i].OriginID,
                    Position = blockTileDatas[i].Position,
                    Priority = blockTileDatas[i].Priority,
                    Icon = _spriteDatabase.GetSpriteByIndex(blockTileDatas[i].OriginID)
                };

                _matchTileFactory.Produce(tileData);
            }
        }

        // This function performs spawning tiles map in gameplay and shuffle original tiles level
        public async UniTask GererateTilesToGameplay(List<BlockTileData> blockTileDatas)
        {
            _gameInput.IsLocked = true;

            ClearLevel();
            _totalTileCount = blockTileDatas.Count;
            var shuffledTiles = ShuffleTileData(blockTileDatas);

            for (int i = 0; i < shuffledTiles.Count; i++)
            {
                TileData tileData = new TileData
                {
                    ID = shuffledTiles[i].OriginID,
                    Position = shuffledTiles[i].Position,
                    Priority = shuffledTiles[i].Priority,
                    Icon = _spriteDatabase.GetSpriteByIndex(shuffledTiles[i].OriginID)
                };

                _matchTiles.Add(_matchTileFactory.Produce(tileData));
            }

            await BuildTileBoard();
            CheckTilesOverlap();
            CheckTilesUnlock();

            _gameInput.IsLocked = false;
        }

        private async UniTask BuildTileBoard()
        {
            for (int i = 0; i < _matchTiles.Count; i++)
            {
                if (_matchTiles[i] is IMatchTileMove tileMove)
                {
                    tileMove.ReturnToOriginalPosition().Forget();
                    await UniTask.DelayFrame(48, cancellationToken: _token);
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), cancellationToken: _token);
            if (_token.IsCancellationRequested)
                return;
        }

        private void CheckTilesOverlap()
        {
            for (int i = 0; i < _matchTiles.Count; i++)
            {
                _matchTiles[i].CheckOverlap();
            }
        }

        public void CheckTilesUnlock()
        {
            for (int i = 0; i < _matchTiles.Count; i++)
            {
                _matchTiles[i].CheckTileUnlock();
            }
        }

        public void TakeOneTile()
        {
            _totalTileCount = _totalTileCount - 1;

            if (_totalTileCount <= 0)
                OnLevelWin?.Invoke();

            else if (_tileMatchRule.IsFull() && _totalTileCount > 0)
                OnLevelLose?.Invoke();
        }

        public void ReturnTile()
        {
            _totalTileCount = _totalTileCount + 1;
        }

        private void ClearLevel()
        {
            for (int i = 0; i < _matchTiles.Count; i++)
            {
                _matchTiles[i].Clear();
            }

            _matchTiles.Clear();
        }

        private List<BlockTileData> ShuffleTileData(List<BlockTileData> blockTileDatas)
        {
            List<BlockTileData> newBlockData = new();

            var originalKeys = blockTileDatas.GroupBy(x => x.OriginID)
                                  .Select(x => x.Key).ToList();
            var shuffledKeys = originalKeys.GetShuffle();

            for (int i = 0; i < blockTileDatas.Count; i++)
            {
                int originalID = blockTileDatas[i].OriginID;
                int originalIndex = originalKeys.IndexOf(originalID);
                int shuffledIndex = originalKeys.GetMappedIndex(originalIndex, shuffledKeys);

                if (shuffledIndex == -1)
                    continue;

                newBlockData.Add(new BlockTileData
                {
                    OriginID = originalKeys[shuffledIndex],
                    Position = blockTileDatas[i].Position,
                    Priority = blockTileDatas[i].Priority
                });
            }

            return newBlockData;
        }

        public void Dispose()
        {
            _tokenSource.Dispose();
            _matchTiles.Clear();
        }
    }
}
