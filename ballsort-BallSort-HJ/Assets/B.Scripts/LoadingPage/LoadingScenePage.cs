using System.Collections;
using UnityEngine;

public class LoadingScenePage : MonoSingleton<LoadingScenePage>
{
    public void Show(string sceneName)
    {
        _progress.Reset(0);
        StartCoroutine(LoadScene(sceneName));
        _mainPage.SetActive(true);
    }

    private IEnumerator LoadScene(string nextSceneName)
    {
        async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress < 0.9f)
                _progressValue = async.progress;
            else
                _progressValue = 1.0f;

            _progress.UpdateProgress(_progressValue);

            if (_progressValue >= 0.9)
            {
                yield return Yielders.Get(0.2f);

                async.allowSceneActivation = true;
            }

            yield return Yielders.Get(0.1f);
        }
    }

    public void ExitPage()
    {
        _mainPage.SetActive(false);
    }

    [SerializeField] private ProgressBarFilled _progress = null;
    [SerializeField] private GameObject _mainPage = null;
    private float _progressValue;
    private AsyncOperation async = null;
}