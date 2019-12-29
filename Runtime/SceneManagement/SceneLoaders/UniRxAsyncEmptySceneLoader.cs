namespace d4160.SceneManagement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UniRx.Async;
    using UnityEngine;

    public class UniTaskEmptySceneLoader : IAsyncSceneLoader
    {
        private List<int> _loadedScenes = new List<int>();

        public async void LoadSceneAsync(
            int buildIdx,
            bool setActiveAsMainScene = false,
            Action<AsyncOperation> onStarted = null,
            Action onCompleted = null,
            bool allowSceneActivation = true,
            AsyncOperationProgress onProgress = null)
        {
            if (buildIdx == -1)
                return;

            if (!_loadedScenes.Contains(buildIdx))
                _loadedScenes.Add(buildIdx);
            else
                return;

            await UniRxAsyncSceneManagementSingleton.LoadSceneAsync(
                buildIdx, setActiveAsMainScene, onStarted,
                onCompleted, allowSceneActivation, onProgress);

            return;
        }

        public async void UnloadSceneAsync(
            int buildIdx,
            Action onCompleted = null)
        {
            if (buildIdx == -1)
                return;

            if (SceneManagementSingleton.IsSceneInBackground(buildIdx))
                return;

            _loadedScenes.Remove(buildIdx);

            await UniRxAsyncSceneManagementSingleton.UnloadSceneAsync(buildIdx, onCompleted);
        }

        public void UnloadAllLoadedScenes(Action onCompleted = null)
        {
            for (int i = _loadedScenes.Count - 1; i >= 0; i--)
            {
                if (onCompleted != null && i == 0)
                    UnloadSceneAsync(_loadedScenes[i], onCompleted);
                else
                    UnloadSceneAsync(_loadedScenes[i]);
            }
        }
    }
}