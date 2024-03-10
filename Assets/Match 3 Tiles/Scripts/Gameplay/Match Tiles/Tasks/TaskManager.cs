using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Tiles.Scripts.Common.Interfaces;
using Match3Tiles.Scripts.GameData.LevelData;
using Newtonsoft.Json;
using Match3Tiles.Scripts.GUI.Screen;
using Match3Tiles.Scripts.Gameplay.Configs;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks
{
    public class TaskManager : IDisposable
    {
        private readonly GameInput _gameInput;
        private readonly TileManager _tileManager;
        private readonly EndGameScreenPanel _endGameScreen;
        private readonly GameStateControlTask _gameStateTask;
        private readonly TileMatchAppender _matchAppender;

        public TaskManager(TileMatchAppender appender, TileManager tileManager
                          , GameInput gameInput, EndGameScreenPanel endGameScreen
                          , GameStateControlTask gameStateControl)
        {
            _tileManager = tileManager;
            _matchAppender = appender;
            _gameInput = gameInput;
            _endGameScreen = endGameScreen;
            _gameStateTask = gameStateControl;

            _gameInput.OnGetMatchBlock = AppendTile;
            _endGameScreen.SetTaskManager(this);
        }

        public void AppendTile(IMatchTile matchTile)
        {
            _matchAppender.Append(matchTile).Forget();
            
            if (_matchAppender.IsAppended)
            {
                _tileManager.TakeOneTile();
                _tileManager.CheckTilesUnlock();
                _matchAppender.IsAppended = false;
            }
        }

        public void GenerateLevel()
        {
            string levelPath = $"LevelDatas/level_{LevelRecorder.Level}";
            TextAsset levelData = Resources.Load<TextAsset>(levelPath);
            
            if (levelData != null)
            {
                string levelDataJson = levelData.text;
                LevelModel levelModel = JsonConvert.DeserializeObject<LevelModel>(levelDataJson);
                _tileManager.GererateTilesToGameplay(levelModel.BlockTileDatas);
            }

            Debug.Log($"Level_{LevelRecorder.Level}");
        }

        public void StartGame()
        {
            GenerateLevel();
            _gameStateTask.StartGame();
            _gameStateTask.PlayGame();
        }

        public void ContinuePlay()
        {
            LevelRecorder.IncreaseLevel();
            GenerateLevel();
            _gameStateTask.ContinuePlayGame();
        }

        public void ReplayGame()
        {
            GenerateLevel();
        }

        public void Dispose()
        {
            _matchAppender.Dispose();
            _tileManager.Dispose();
        }
    }
}
