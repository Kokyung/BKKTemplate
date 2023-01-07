using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BKK.SceneManagement
{
    public static class SceneUtility
    {
        public static float loadProgress = 0;
        
        public static async UniTask LoadSceneAsync(string sceneName, Task beforeLoading = null, Task afterLoading = null,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            loadProgress = 0;
            
            await beforeLoading;
            
            var loadOps = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            while (!loadOps.isDone)
            {
                loadProgress = loadOps.progress;
                await UniTask.Yield();
            }
            
            await loadOps;

            await afterLoading;
        }
    }
}

