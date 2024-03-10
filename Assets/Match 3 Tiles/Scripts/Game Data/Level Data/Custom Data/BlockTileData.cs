using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Match3Tiles.Scripts.GameData.LevelData.Converters;

namespace Match3Tiles.Scripts.GameData.LevelData.CustomData
{
    [JsonConverter(typeof(BlockTileDataConverter))]
    public class BlockTileData
    {
        public int OriginID;
        public int Priority;
        public Vector3 Position;
    }
}
