using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Tiles.Scripts.Gameplay.Factories;
using Match3Tiles.Scripts.GameData.LevelData.CustomData;
using Match3Tiles.Scripts.Databases;
using Match3Tiles.Scripts.Utils;
using Match3Tiles.Scripts.Common.Interfaces;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks
{
    public class TileManager : IDisposable
    {
        private readonly TileMatchRule _tileMatchRule;
        private readonly TileSpriteDatabase _spriteDatabase;
        private readonly MatchTileFactory _matchTileFactory;

        private int _totalTileCount;
        private List<IMatchTile> _matchTiles;

        public Action OnLevelWin;
        public Action OnLevelLose;

        public TileManager(MatchTileFactory tileFactory, TileMatchRule tileMatchRule, TileSpriteDatabase spriteDatabase)
        {
            _matchTiles = new();
            _tileMatchRule = tileMatchRule;
            _spriteDatabase = spriteDatabase;
            _matchTileFactory = tileFactory;
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
        public void GererateTilesToGameplay(List<BlockTileData> blockTileDatas)
        {
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

            CheckTilesOverlap();
            CheckTilesUnlock();
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
            _matchTiles.Clear();
        }
    }
}
