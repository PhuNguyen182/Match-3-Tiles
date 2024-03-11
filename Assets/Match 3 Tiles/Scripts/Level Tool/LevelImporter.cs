using Match3Tiles.Scripts.Databases;
using Match3Tiles.Scripts.GameData.LevelData;
using Match3Tiles.Scripts.Gameplay.Factories;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Entities;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3Tiles.Scripts.LevelTool
{
    public class LevelImporter
    {
        private readonly Transform _tileContainer;
        private readonly TileSpriteDatabase _spriteDatabase;
        private readonly MatchTileBlock _tileBlock;
        private readonly MatchTileFactory _tileFactory;
        private readonly TileManager _tileGenerator;

        public LevelImporter(MatchTileBlock tileBlock, TileSpriteDatabase spriteDatabase, Transform tileContainer)
        {
            _tileBlock = tileBlock;
            _spriteDatabase = spriteDatabase;
            _tileContainer = tileContainer;
            _tileFactory = new(_tileBlock, null);
            _tileGenerator = new(_tileFactory, null, _spriteDatabase, null);
        }

        public LevelImporter ImportLevel(LevelModel levelModel)
        {
            _tileGenerator.GererateTilesToImport(levelModel.BlockTileDatas);
            return this;
        }

        public void Import()
        {

        }
    }
}
