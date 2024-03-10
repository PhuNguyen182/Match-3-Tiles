using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Tiles.Scripts.Common.Interfaces;
using Match3Tiles.Scripts.Gameplay.MatchTiles;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Entities;

namespace Match3Tiles.Scripts.Gameplay.Factories 
{
    public class MatchTileFactory : IFactory<TileData, MatchTileBlock>
    {
        private readonly Transform _tileContainer;
        private readonly MatchTileBlock _tileBlockPrefab;

        public MatchTileFactory(MatchTileBlock tileBlockPrefab, Transform tileContainer)
        {
            _tileContainer = tileContainer;
            _tileBlockPrefab = tileBlockPrefab;
        }

        public MatchTileBlock Produce(TileData param)
        {
            MatchTileBlock tileBlock = SimplePool.Spawn(_tileBlockPrefab, _tileContainer
                                                        , param.Position, Quaternion.identity);

            tileBlock.SetTileData(param);
            tileBlock.SetSortingOrder(param.Priority);
            tileBlock.IsSlottedInOrder = false;
            tileBlock.SetColliderEnable(true);

            return tileBlock;
        }
    }
}
