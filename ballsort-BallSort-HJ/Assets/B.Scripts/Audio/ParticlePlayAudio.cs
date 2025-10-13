using Sirenix.OdinInspector;
using UnityEngine;

public class ParticlePlayAudio : MonoBehaviour
{
    [Button]
    public void Play()
    {
        JobUtils.Delay(_delay, DelayPlay);
    }

    private void DelayPlay()
    {
        if (_clip != null)
        {
            AudioManager.Instance.PlaySoundEffect(_clip);
        }
    }

    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _delay = 0f;
}