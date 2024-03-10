using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Match3Tiles.Scripts.SceneUtils;
using Cysharp.Threading.Tasks;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks;

namespace Match3Tiles.Scripts.GUI.Screen 
{
    public class WinPanel : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button quitButton;

        private TaskManager _taskManager;

        private void Awake()
        {
            continueButton.onClick.AddListener(ContinueGame);
            quitButton.onClick.AddListener(QuitGame);
        }

        public void SetTaskManager(TaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        private void ContinueGame()
        {
            _taskManager.ContinuePlay();
            Close();
        }

        private void QuitGame()
        {
            Close();
            SceneLoader.LoadScene(SceneLoader.MAINHOME, trigger: () => true).Forget();
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
    } 
}
