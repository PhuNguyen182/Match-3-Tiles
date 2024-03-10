using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Match3Tiles.Scripts.SceneUtils
{
    public class SceneLoader
    {
        public const string LOADING = "Loading";
        public const string MAINHOME = "Mainhome";
        public const string GAMEPLAY = "Gameplay";

        public static async UniTask LoadScene(string sceneName, IProgress<float> progress, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(sceneName, loadMode);
            await sceneOperation.ToUniTask(progress);
        }

        public static async UniTask LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single
            , Action<float> progress = null, Func<bool> trigger = null, CancellationToken cancellationToken = default)
        {
            await UniTask.NextFrame(cancellationToken);

            AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(sceneName, loadMode);
            sceneOperation.allowSceneActivation = false;

            while (!sceneOperation.isDone)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                progress?.Invoke(sceneOperation.progress);

                if(sceneOperation.progress >= 0.9f)
                {
                    if (trigger.Invoke())
                        sceneOperation.allowSceneActivation = true;
                }

                await UniTask.NextFrame(cancellationToken);
            }
        }
    }
}
