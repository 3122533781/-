using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoyBean.Procedure
{
    public class ProcedureWork
    {
        public Action OnCompleted = delegate { };

        public List<Procedure> ParallelProcedures; //并行的工作

        public void DoWork()
        {
            for (int i = 0; i < ParallelProcedures.Count; i++)
            {
                ParallelProcedures[i].OnCompleted += OnProcedureCompleted;
                ParallelProcedures[i].StartWork();
            }
        }

        public void AddProcedure(Procedure value)
        {
            ParallelProcedures.Add(value);
        }

        public ProcedureWork()
        {
            ParallelProcedures = new List<Procedure>();
        }

        private void OnProcedureCompleted(Procedure procedure)
        {
            procedure.OnCompleted -= OnProcedureCompleted;
            if (IsAllProcedureCompleted())
            {
                OnCompleted.Invoke();
            }
        }

        private bool IsAllProcedureCompleted()
        {
            foreach (Procedure item in ParallelProcedures)
            {
                if (!item.IsCompleted)
                {
                    return false;
                }
            }

            return true;
        }
    }
}