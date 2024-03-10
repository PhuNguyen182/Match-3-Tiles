using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3Tiles.Scripts.Databases;
using Match3Tiles.Scripts.Common.Interfaces;
using Match3Tiles.Scripts.Gameplay.Factories;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Entities;
using Match3Tiles.Scripts.GUI.Screen;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks
{
    public class GameController : MonoBehaviour, IDisposable, IService
    {
        [SerializeField] private GameInput gameInput;
        [SerializeField] private MatchOrder matchOrder;
        [SerializeField] private MatchTileBlock tileBlockPrefab;
        [SerializeField] private TileSpriteDatabase spriteDatabase;
        [SerializeField] private Transform tileContainer;
        [SerializeField] private EndGameScreenPanel endGameScreen;

        private TileMatchRule _matchRule;
        private TileMatchAppender _matchAppender;
        private MatchTileFactory _matchTileFactory;
        private GameStateControlTask _gameStateControl;
        private TileManager _tileGenerator;
        private TaskManager _taskManager;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            StartGame();
        }

        public void Initialize()
        {
            _matchRule = new();
            _matchAppender = new(matchOrder, _matchRule);
            _matchTileFactory = new(tileBlockPrefab, tileContainer);
            _tileGenerator = new(_matchTileFactory, _matchRule, spriteDatabase);
            _gameStateControl = new(_tileGenerator, endGameScreen);
            _taskManager = new(_matchAppender, _tileGenerator, gameInput, endGameScreen, _gameStateControl);
        }

        private void StartGame()
        {
            _taskManager.StartGame();
        }

        public void Dispose()
        {
            _taskManager.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
