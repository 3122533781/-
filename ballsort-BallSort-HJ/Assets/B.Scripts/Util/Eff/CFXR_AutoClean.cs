using UnityEngine;
using static CartoonFX.CFXR_Effect;

public class CFXR_AutoClean : MonoBehaviour
{
    public ClearBehavior clearBehavior = ClearBehavior.Disable;
    public ClearType clearType = ClearType.Finish;
    public float delayTime = 0;

    ParticleSystem rootParticleSystem;
    int CHECK_EVERY_N_FRAME = 20;
    int GlobalStartFrameOffset = 0;
    int startFrameOffset;
    float time = 0;

    void Awake()
    {
        startFrameOffset = GlobalStartFrameOffset++;
        time = 0;
    }

    void Update()
    {
        if (delayTime > time) time += Time.deltaTime;
        if (clearType == ClearType.Finish)
        {
            if (rootParticleSystem == null)
            {
                rootParticleSystem = GetComponent<ParticleSystem>();
                if (rootParticleSystem == null) rootParticleSystem = GetComponentInChildren<ParticleSystem>();
            }
            if ((Time.renderedFrameCount + startFrameOffset) % CHECK_EVERY_N_FRAME == 0)
            {
                if (!rootParticleSystem.IsAlive(true))
                {
                    CheckClean();
                }
            }
        }
        else if (clearType == ClearType.Delay)
        {
            if (delayTime <= time)
            {
                CheckClean();
            }
        }
    }

    void CheckClean()
    {
        if (clearBehavior == ClearBehavior.Destroy)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

}
public enum ClearType
{
    Finish,
    Delay
}