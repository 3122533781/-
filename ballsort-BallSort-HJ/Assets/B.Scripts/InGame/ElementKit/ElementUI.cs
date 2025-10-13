using Fangtang;
using UnityEngine;

public class ElementUI<T> : ElementBehavior<T> where T : SceneElement
{
    protected override void HandleSub()
    {
        ElementSubUI<T>[] subUIs = GetComponentsInChildren<ElementSubUI<T>>(true);

        for (int i = 0; i < subUIs.Length; i++)
        {
            subUIs[i].Init(Context);
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        gameObject.SetActive(false);
    }

    public override void Activate()
    {
        base.Activate();
        gameObject.SetActive(true);
    }
}