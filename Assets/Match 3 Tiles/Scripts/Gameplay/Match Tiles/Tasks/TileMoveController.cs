using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using Match3Tiles.Scripts.Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks
{
    public class TileMoveController
    {
        private readonly TileMatchAppender _tileAppender;

        public TileMoveController(TileMatchAppender tileAppender) => _tileAppender = tileAppender;

        public async UniTask ExecuteMatchedTiles()
        {
            using PooledObject<List<UniTask>> listPool = ListPool<UniTask>.Get(out List<UniTask> moveList);
            using PooledObject<List<IMatchTile>> poolTiles = ListPool<IMatchTile>.Get(out List<IMatchTile> tiles);

            for (int i = 0; i < _tileAppender.MatchedTiles.Count; i++)
            {
                tiles.Add(_tileAppender.MatchedTiles[i]);
                if (_tileAppender.MatchedTiles[i] is IMatchTileMove tileMove)
                    moveList.Add(tileMove.MoveTo(_tileAppender.FreePoint.position));
            }

            await UniTask.WhenAll(moveList);

            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Clear();
            }

            _tileAppender.MatchedTiles.Clear();
        }

        public UniTask UpdateTilePositions()
        {
            using PooledObject<List<UniTask>> listPool = ListPool<UniTask>.Get(out List<UniTask> moveList);
            
            for (int i = 0; i < _tileAppender.TileOrder.Count; i++)
            {
                if (_tileAppender.TileOrder[i] is IMatchTileMove matchTileMove)
                    moveList.Add(matchTileMove.MoveTo(_tileAppender.Destinations[i]));
            }

            return UniTask.WhenAll(moveList);
        }
    }
}
