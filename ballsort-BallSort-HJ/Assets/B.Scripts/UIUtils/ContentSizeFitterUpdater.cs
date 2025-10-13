using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ContentSizeFitter))]
public class ContentSizeFitterUpdater : MonoBehaviour
{
    [SerializeField] public bool IsVertical = true;
    
    private void Update()
    {
        if (IsVertical)
        {
            _fitter.Get(gameObject).SetLayoutVertical();
        }
        else
        {
            _fitter.Get(gameObject).SetLayoutHorizontal();
        }

        
    }
    
    private DelayGetComponent<ContentSizeFitter> _fitter = new DelayGetComponent<ContentSizeFitter>();
}