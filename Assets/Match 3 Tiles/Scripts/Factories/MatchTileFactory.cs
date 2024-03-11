using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Tiles.Scripts.Common.Interfaces;
using Match3Tiles.Scripts.Gameplay.MatchTiles;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Entities;
using Match3Tiles.Scripts.LevelTool;

namespace Match3Tiles.Scripts.Gameplay.Factories 
{
    public class MatchTileFactory : IFactory<TileData, MatchTileBlock>
    {
        private readonly MatchOrder _matchOrder;
        private readonly MatchTileBlock _tileBlockPrefab;
        private readonly Transform _tileContainer;

        public MatchTileFactory(MatchTileBlock tileBlockPrefab, MatchOrder matchOrder)
        {
            _matchOrder = matchOrder;
            _tileBlockPrefab = tileBlockPrefab;

            _tileContainer = _matchOrder == null 
                            ? Object.FindAnyObjectByType<LevelBuilder>().transform 
                            : _matchOrder.TileContainer;
        }

        public MatchTileBlock Produce(TileData param)
        {
            Vector3 spawnPosition = _matchOrder != null 
                                    ? _matchOrder.StartPoint.position 
                                    : param.Position;

            MatchTileBlock tileBlock = SimplePool.Spawn(_tileBlockPrefab, _tileContainer
                                                        , spawnPosition, Quaternion.identity);

            tileBlock.SetTileData(param);
            tileBlock.SetSortingOrder(param.Priority);
            tileBlock.IsSlottedInOrder = false;
            tileBlock.SetColliderEnable(true);

            return tileBlock;
        }
    }
}
