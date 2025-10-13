using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    private void OnEnable()
    {
        if (AudioManager.IsInited)
            AudioManager.Instance.PlayBGM(_bgmClip);
    }

    [SerializeField] private AudioClip _bgmClip = null;
}