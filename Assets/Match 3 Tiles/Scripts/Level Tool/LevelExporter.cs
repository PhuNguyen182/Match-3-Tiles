using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Newtonsoft.Json;
using Match3Tiles.Scripts.GameData.LevelData;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Entities;
using Match3Tiles.Scripts.GameData.LevelData.CustomData;
using UnityEngine;

namespace Match3Tiles.Scripts.LevelTool
{
    public class LevelExporter
    {
        private readonly LevelModel _levelModel = new();

        public LevelExporter Clear()
        {
            _levelModel.Clear();
            return this;
        }

        public LevelExporter BuildLevel(List<MatchTileBlock> tileBlocks)
        {
            for (int i = 0; i < tileBlocks.Count; i++)
            {
                _levelModel.BlockTileDatas.Add(new BlockTileData
                {
                    OriginID = tileBlocks[i].ID,
                    Position = tileBlocks[i].Position,
                    Priority = tileBlocks[i].Priority
                });
            }

            return this;
        }

        public void Export(string level)
        {
            string levelPath = $"Assets/Match 3 Tiles/Resources/LevelDatas/{level}.txt";
            string json = JsonConvert.SerializeObject(_levelModel, Formatting.None);

            using StreamWriter writer = new StreamWriter(levelPath);
            writer.Write(json);
            writer.Close();

#if UNITY_EDITOR
            Debug.Log(json);
            AssetDatabase.ImportAsset(levelPath);
#endif
        }
    }
}
