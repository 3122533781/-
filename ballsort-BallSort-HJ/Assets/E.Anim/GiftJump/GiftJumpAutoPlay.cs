using UnityEngine;

public class GiftJumpAutoPlay : MonoBehaviour
{
    private void OnEnable()
    {
        AnimatorHelper.SetParameter(GetComponent<Animator>(), "IsJumping", true);
    }
}