using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soybean.GameWorker
{
    public static class GameWorkerGUIMain
    {
        public static void Draw(GameWorker context)
        {
            GUILayout.Label("WorkerFlow:");


            for (int i = 0; i < context.Model.ProcessUnits.Count; i++)
            {
                GameWorkerUnit unit = context.Model.ProcessUnits[i];
                GUI.color = GetColor(unit.State);

                GUILayout.BeginHorizontal();
                GUILayout.Toggle(unit.State == UnitState.Pass, $"");
                GUILayout.Label($"{unit.Name}:");
                GUILayout.Label(unit.IsNecessary ? "Necessary" : "Not");
                GUILayout.EndHorizontal();

                GUI.color = Color.white;
            }
        }

        public static Color GetColor(UnitState state)
        {
            switch (state)
            {
                case UnitState.Standby:
                    return Color.white;
                case UnitState.Pass:
                    return Color.green;
                case UnitState.Active:
                    return Color.cyan;
                case UnitState.Remove:
                    return Color.gray;
                default:
                    return Color.white;
            }
        }
    }
}