using UnityEngine;

public class UICamera : MonoSingleton<UICamera>
{
    [SerializeField] public Camera Camera = null;
    [SerializeField] public Camera MainCamera = null;
}