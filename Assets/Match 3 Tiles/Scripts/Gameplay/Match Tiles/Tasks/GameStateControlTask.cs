using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stateless;
using Match3Tiles.Scripts.GUI.Screen;
using Cysharp.Threading.Tasks;

namespace Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks
{
    public class GameStateControlTask
    {
        public enum GameState
        {
            Start,
            Playing,
            EndGame,
            Quit
        }

        public enum TriggerAction
        {
            StartGame,
            PlayGame,
            EndGame,
            Continue,
            Replay,
            QuitGame,
        }

        private readonly TileManager _tileManager;
        private readonly EndGameScreenPanel _endGameScreen;

        private StateMachine<GameState, TriggerAction> _gameStateMachine;
        private StateMachine<GameState, TriggerAction>.TriggerWithParameters _startTrigger;
        private StateMachine<GameState, TriggerAction>.TriggerWithParameters<bool> _endTrigger;

        private bool _canReplay;

        public GameStateControlTask(TileManager tileManager, EndGameScreenPanel endGameScreen)
        {
            _tileManager = tileManager;
            _endGameScreen = endGameScreen;

            _tileManager.OnLevelWin = () => EndGame(true);
            _tileManager.OnLevelLose = () => EndGame(false);

            CreateStateMachine();
        }

        private void CreateStateMachine()
        {
            _gameStateMachine = new StateMachine<GameState, TriggerAction>(GameState.Start);
            _startTrigger = _gameStateMachine.SetTriggerParameters(TriggerAction.StartGame);
            _endTrigger = _gameStateMachine.SetTriggerParameters<bool>(TriggerAction.EndGame);

            _gameStateMachine.Configure(GameState.Start)
                             .Permit(TriggerAction.PlayGame, GameState.Playing);

            _gameStateMachine.Configure(GameState.Playing)
                             .OnEntryFrom(_startTrigger.Trigger, OnGameplay)
                             .OnEntryFrom(TriggerAction.Continue, ContinuePlaygame)
                             .OnEntryFrom(TriggerAction.Replay, OnReplayGame)
                             .Permit(_endTrigger.Trigger, GameState.EndGame)
                             .Permit(TriggerAction.QuitGame, GameState.Quit);

            _gameStateMachine.Configure(GameState.EndGame)
                             .OnEntryFrom(_endTrigger, b => OnEndGame(b).Forget())
                             .Permit(TriggerAction.Continue, GameState.Playing)
                             .Permit(TriggerAction.Replay, GameState.Playing)
                             .Permit(TriggerAction.QuitGame, GameState.Quit);

            _gameStateMachine.Configure(GameState.Quit)
                             .OnEntry(OnQuitGame);
        }

        public void StartGame()
        {
            if (_gameStateMachine.CanFire(_startTrigger.Trigger))
            {
                _gameStateMachine.Fire(_startTrigger.Trigger);
            }
        }

        public void PlayGame()
        {
            if (_gameStateMachine.CanFire(TriggerAction.PlayGame))
            {
                _gameStateMachine.Fire(TriggerAction.PlayGame);
            }
        }

        public void EndGame(bool isWin)
        {
            if (_gameStateMachine.CanFire(_endTrigger.Trigger))
            {
                _gameStateMachine.Fire(_endTrigger, isWin);
            }
        }

        public void ContinuePlayGame()
        {
            if (_gameStateMachine.CanFire(TriggerAction.Continue))
            {
                _gameStateMachine.Fire(TriggerAction.Continue);
            }
        }

        public void Replay()
        {
            if (_gameStateMachine.CanFire(TriggerAction.Replay))
            {
                _gameStateMachine.Fire(TriggerAction.Replay);
            }
        }

        public void QuitGame()
        {
            if (_gameStateMachine.CanFire(TriggerAction.QuitGame))
            {
                _gameStateMachine.Fire(TriggerAction.QuitGame);
            }
        }

        private void OnGameplay()
        {
            Debug.Log("Game Playing");
        }

        private void ContinuePlaygame()
        {
            Debug.Log("Game Continue");
        }

        private void OnReplayGame()
        {
            Debug.Log("Game Replay");
        }

        private async UniTask OnEndGame(bool isWin)
        {
            Debug.Log(isWin ? "Game Complete" : "Game Over");

            if (isWin)
            {
                _endGameScreen.ShowWinPanel();
            }

            else
            {
                _canReplay = await _endGameScreen.ShowLosePanel();
                
                if (_canReplay)
                    Replay();
                
                else
                    QuitGame();
            }
        }

        private void OnQuitGame()
        {
            Debug.Log("Quit");
        }
    }
}
