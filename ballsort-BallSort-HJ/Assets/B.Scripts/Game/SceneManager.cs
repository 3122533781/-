using UnityEngine.SceneManagement;

public class SceneManager
{
    public static string CurrentScene { get; private set; }
    public static string PreviousScene { get; private set; }

    public const string GameStageScene = "InGame";
    public const string HomeScene = "Home";
    public const string LoadingScene = "Loading";

    public static bool PreviousIsHome()
    {
        return PreviousScene.Equals(HomeScene);
    }

    public SceneManager(string currentSceneName)
    {
        CurrentScene = currentSceneName;
        PreviousScene = "";
    }

    public string GetActiveScene()
    {
        return CurrentScene;
    }

    public static void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        PreviousScene = CurrentScene;
        CurrentScene = sceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, mode);
    }
}