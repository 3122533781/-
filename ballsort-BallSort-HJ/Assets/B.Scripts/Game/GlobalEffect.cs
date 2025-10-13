using UnityEngine;

public class GlobalEffect : MonoSingleton<GlobalEffect>
{
    public void PlayGoldStarPointEffect(Vector3 worldPos)
    {
        GameObject effect = PoolManager.Instance.CreateObject("Gold_Point", 1f);
        effect.transform.position = worldPos;
    }
    
    public void PlayGoldStarButtonEffect(Vector3 worldPos)
    {
        GameObject effect = PoolManager.Instance.CreateObject("Gold_Button", 1f);
        effect.transform.position = worldPos;
    }
    
    public void PlayTipClickStarInEffect(Vector3 worldPos)
    {
        GameObject effect = PoolManager.Instance.CreateObject("TipClick_in", 2f);
        effect.transform.position = worldPos;
    }
}
