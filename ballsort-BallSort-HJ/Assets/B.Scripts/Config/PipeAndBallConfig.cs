using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02.Scripts.Config
{
    public class PipeAndBallConfig : ScriptableConfigGroup<PipeAndBallData, PipeAndBallConfig>
    {
    }

    [Serializable]
    public class PipeAndBallData : IConfig
    {
        public int intID;
        public int subType;
        public int ID => intID;
        public int ball;
        public int pipeW;
        public int pipeH;
        public int pipeHSpace;
        public int pipeWSpace;
    }
}