using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Match3Tiles.Scripts.GameData.LevelData.CustomData;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Entities;
using Match3Tiles.Scripts.GameData.LevelData;
using Match3Tiles.Scripts.Databases;
using Newtonsoft.Json;
using Match3Tiles.Scripts.Utils;

namespace Match3Tiles.Scripts.LevelTool
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private Transform tileContainer;
        [SerializeField] private MatchTileBlock tileBlockPrefab;
        [SerializeField] private TileSpriteDatabase spriteDatabase;

        private LevelExporter _levelExporter;
        private LevelImporter _levelImporter;

        [Button]
        public void ExportLevel(int level)
        {
            _levelExporter = new();
            List<MatchTileBlock> matchTiles = new();

            for (int i = 0; i < tileContainer.childCount; i++)
            {
                if (tileContainer.GetChild(i).TryGetComponent(out MatchTileBlock block))
                    matchTiles.Add(block);
            }

            _levelExporter.Clear().BuildLevel(matchTiles).Export($"level_{level}");
        }

        [Button]
        public void ImportLevel(int level)
        {
            _levelImporter = new(tileBlockPrefab, spriteDatabase, tileContainer);
            TextAsset data = Resources.Load<TextAsset>($"LevelDatas/level_{level}");
            
            if (data == null)
                return;
            
            LevelModel model = JsonConvert.DeserializeObject<LevelModel>(data.text);
            _levelImporter.ImportLevel(model);
        }
    }
}
