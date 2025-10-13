using System;
using System.Collections.Generic;

public class RolePlotConfig : ScriptableConfigGroup<RolePlotData, RolePlotConfig>
{
}

[Serializable]
public class RolePlotData : IConfig
{
    public int ID => Id;
    public int Id;

    public int Bg;
    public int Txt;
    public int Speak;
    public List<string> Role1Act;
    public List<string> Role2Act;
    public int Special;
    public int AutoNext;
    public float GaussianBlur;
}
