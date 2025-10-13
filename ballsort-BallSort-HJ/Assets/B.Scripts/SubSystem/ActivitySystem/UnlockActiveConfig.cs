using System;
using ProjectSpace.Lei31Utils.Scripts.Utils2.Dialog;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectSpace.BubbleMatch.Scripts.SubSystem.ActivitySystem
{
    public class UnlockActiveConfig : ScriptableConfigGroup<UnlockActiveData, UnlockActiveConfig>
    {
    }

    [Serializable]
    public class UnlockActiveData : IConfig
    {
        public DialogName id;
        public int unlockLevel;

        public int ID => (int) id;
    }
}