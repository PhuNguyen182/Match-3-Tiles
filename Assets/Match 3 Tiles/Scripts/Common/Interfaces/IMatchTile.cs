using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Match3Tiles.Scripts.Gameplay.MatchTiles;

namespace Match3Tiles.Scripts.Common.Interfaces
{
    public interface IMatchTile
    {
        public int ID { get; }
        public int Priority { get; }
        public bool IsLocked { get; }
        public bool IsSlottedInOrder { get; set; }
        
        public Vector3 Position { get; }
        public TileData TileData { get; }

        public void Clear();
        public void CheckOverlap();
        public void CheckTileUnlock();
        public void SetColliderEnable(bool enable);
        public void SetSortingOrder(int sortingOrder);
        public void SetTileData(TileData tileData);
    }
}
