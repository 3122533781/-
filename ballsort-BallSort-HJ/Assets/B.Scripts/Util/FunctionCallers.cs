using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CallerInfo
{
    public KeyCode Key = KeyCode.Space;
    public string MonoBehaviourName;
    public string MethodName;
}

public class FunctionCallers : MonoBehaviour 
{
    [SerializeField]
    public List<CallerInfo> All;

#if UNITY_EDITOR
    public void Update()
    {
        if (All == null) return;

        for (int i = 0; i < All.Count; i++)
        {
            if (Input.GetKeyDown(All[i].Key))
            {
                gameObject.SendMessage(All[i].MethodName,
                    this, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
#endif
}
