using UnityEngine;

public class SoomthLookerCamera : MonoBehaviour
{
    private void Update()
    {
        if (_transform == null)
        {
            _transform = transform;
            _cameraTransform = Camera.main.transform;
        }

        _transform.forward = _cameraTransform.forward;
    }

    private Transform _transform;
    private Transform _cameraTransform;
}