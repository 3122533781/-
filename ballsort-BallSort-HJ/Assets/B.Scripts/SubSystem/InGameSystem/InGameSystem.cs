using Models;
using System;

public class InGameSystem : GameSystem
{
    public Action<bool, bool> OnSelectEnd;

    public bool IsPlaying;
    public bool IsClickPause;

    public override void Init()
    {

    }

    public override void Destroy()
    {
        OnSelectEnd = null;
    }
}
