using System;
using System.Collections;
using System.Collections.Generic;
using Match3Tiles.Scripts.GameData.LevelData.CustomData;

namespace Match3Tiles.Scripts.GameData.LevelData
{
    [Serializable]
    public class LevelModel
    {
        public List<BlockTileData> BlockTileDatas;

        public LevelModel()
        {
            BlockTileDatas = new();
        }

        public void Clear()
        {
            BlockTileDatas.Clear();
        }
    }
}
