using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Match3Tiles.Scripts.SceneUtils;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks;

namespace Match3Tiles.Scripts.GUI.Screen
{
    public class LosePanel : MonoBehaviour
    {
        [SerializeField] private Button replayButton;
        [SerializeField] private Button quitButton;

        private TaskManager _taskManager;
        private UniTaskCompletionSource<bool> _source;

        private void Awake()
        {
            replayButton.onClick.AddListener(ReplayGame);
            quitButton.onClick.AddListener(QuitGame);
        }

        public void SetTaskManager(TaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        public UniTask<bool> Show()
        {
            gameObject.SetActive(true);
            _source = new UniTaskCompletionSource<bool>();
            return _source.Task;
        }

        private void ReplayGame()
        {
            _source.TrySetResult(true);
            _taskManager.ReplayGame();
            Close();
        }

        private void QuitGame()
        {
            _source.TrySetResult(false);
            SceneLoader.LoadScene(SceneLoader.MAINHOME, trigger: () => true).Forget();
            Close();
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
