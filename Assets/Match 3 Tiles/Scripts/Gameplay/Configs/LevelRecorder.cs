using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3Tiles.Scripts.Gameplay.Configs
{
    public static class LevelRecorder
    {
        private const string LEVEL_KEY = "LevelKey";

        public const int MAX_LEVEL = 10;

        public static int Level
        {
            get => PlayerPrefs.GetInt(LEVEL_KEY, 1);
            private set => PlayerPrefs.SetInt(LEVEL_KEY, value);
        }

        public static void IncreaseLevel()
        {
            Level = Level + 1;

            if (Level > MAX_LEVEL)
                Level = MAX_LEVEL;
        }
    }
}
