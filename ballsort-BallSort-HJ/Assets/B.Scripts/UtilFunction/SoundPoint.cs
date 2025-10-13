using UnityEngine;

public class SoundPoint : PoolAbstractObject
{
    public static void PlayClip(float duration, AudioClip clip, float volume = 1)
    {
        SoundPoint point = Create<SoundPoint>(duration);
        point._source.clip = clip;
        point._source.volume = volume;
        point._source.Play();
    }


    [SerializeField] private AudioSource _source = null;
}