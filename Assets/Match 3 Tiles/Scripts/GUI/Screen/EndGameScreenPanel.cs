using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Match3Tiles.Scripts.Gameplay.MatchTiles.Tasks;

namespace Match3Tiles.Scripts.GUI.Screen
{
    public class EndGameScreenPanel : MonoBehaviour
    {
        [SerializeField] private WinPanel winPanel;
        [SerializeField] private LosePanel losePanel;

        public void SetTaskManager(TaskManager taskManager)
        {
            winPanel.SetTaskManager(taskManager);
            losePanel.SetTaskManager(taskManager);
        }

        public void ShowWinPanel()
        {
            winPanel.gameObject.SetActive(true);
        }

        public UniTask<bool> ShowLosePanel()
        {
            return losePanel.Show();
        }
    }
}
