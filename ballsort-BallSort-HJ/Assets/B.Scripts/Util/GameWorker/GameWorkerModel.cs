using System.Collections.Generic;
using UnityEngine;

namespace Soybean.GameWorker
{
    public class GameWorkerModel
    {
        public bool IsWorking { get; set; }

        public List<GameWorkerUnit> ProcessUnits { get; set; }

        public int ActiveNessUnitIndex { get; set; }
        public GameWorkerUnit ActiveNessUnit => ProcessUnits[ActiveNessUnitIndex];
        public List<GameWorkerUnit> ActiveAttachedUnits { get; set; }

        public bool IsFinish => ActiveNessUnitIndex >= ProcessUnits.Count;

        public void PassUnit(GameWorkerUnit unit)
        {
            unit.State = UnitState.Pass;
            if (unit.IsNecessary)
            {
                PassNessUnit();
            }
            else
            {
                ActiveAttachedUnits.Remove(unit);
            }
        }

        public void PassNessUnit()
        {
            ActiveNessUnitIndex = FindNearestNessIndex();
            
            for (int i = 0; i < ActiveAttachedUnits.Count; i++)
            {
                ActiveAttachedUnits[i].State = UnitState.Remove;
            }
            
            if (ActiveNessUnitIndex >= ProcessUnits.Count)
            {
                Debug.Log("Test Finish...");
            }
            else
            {
                ActiveAttachedUnits.Clear();
                ActiveAttachedUnits.AddRange(FindAttachUnits());
                for (int i = 0; i < ActiveAttachedUnits.Count; i++)
                {
                    ActiveAttachedUnits[i].State = UnitState.Active;
                }
                ActiveNessUnit.State = UnitState.Active;
            }
        }

        public void InitStart()
        {
            ActiveNessUnit.State = UnitState.Active;
        }

        public int FindNearestNessIndex()
        {
            int findIndex = ActiveNessUnitIndex + 1;
            while (findIndex < ProcessUnits.Count)
            {
                if (ProcessUnits[findIndex].IsNecessary)
                {
                    return findIndex;
                }

                findIndex++;
            }

            return findIndex;
        }

        public List<GameWorkerUnit> FindAttachUnits()
        {
            List<GameWorkerUnit> result = new List<GameWorkerUnit>();
            int startIndex = ActiveNessUnitIndex - 1;
            while (startIndex > 0)
            {
                if (ProcessUnits[startIndex].IsNecessary)
                {
                    break;
                }
                else
                {
                    result.Add(ProcessUnits[startIndex]);
                }

                startIndex--;
            }

            return result;
        }

        public GameWorkerModel()
        {
            ActiveAttachedUnits = new List<GameWorkerUnit>();
            ActiveNessUnitIndex = 0;
            IsWorking = false;
            ProcessUnits = GameWorkerConfig.Instance.GetCopys();
        }
    }
}