using UnityEngine;

public class AutoShowPopup : MonoBehaviour
{
    private void OnEnable()
    {
        AnimatorHelper.SetTrigger(GetComponent<Animator>(), "Show");
    }
}