using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoyBean.Procedure
{
    public class ProcedureWorkerStation
    {
        public Action OnWorkerCompleted = delegate { };
        public bool IsWorking { get; private set; }

        public float CompletionPercentage { get; private set; }

        public void StartWork()
        {
            IsWorking = true;
            _currentDoingWork = _works.Dequeue();
            _currentDoingWork.OnCompleted += ChangeNextWork;
            _currentDoingWork.DoWork();
        }

        public void Add(Procedure procedure, AddProcedureType type = AddProcedureType.Serial)
        {
            if (IsWorking)
                throw new Exception("Working cannot add procedure");

            if (type == AddProcedureType.Parallel)
            {
                HandleParallelAdd(procedure);
            }
            else if (type == AddProcedureType.Serial)
            {
                HandleSerialAdd(procedure);
            }

            _sumProcedurePoint += procedure.Point;
            procedure.OnCompleted += ProcedureCompleted;
        }

        private void ProcedureCompleted(Procedure procedure)
        {
            procedure.OnCompleted -= ProcedureCompleted;

            _completedProcedurePoint += procedure.Point;

            CompletionPercentage = (float) _completedProcedurePoint / (float) _sumProcedurePoint;
        }

        private void HandleSerialAdd(Procedure procedure)
        {
            ProcedureWork work = new ProcedureWork();
            work.AddProcedure(procedure);
            _works.Enqueue(work);
        }

        private void HandleParallelAdd(Procedure procedure)
        {
            if (_works.Count == 0)
            {
                _works.Enqueue(new ProcedureWork());
            }

            _works.Peek().AddProcedure(procedure);
        }

        private void ChangeNextWork()
        {
            _currentDoingWork.OnCompleted -= ChangeNextWork;
            if (_works.Count == 0)
            {
                Reset();
                IsWorking = false;
                OnWorkerCompleted.Invoke();
            }
            else
            {
                _currentDoingWork = _works.Dequeue();
                _currentDoingWork.OnCompleted += ChangeNextWork;
                _currentDoingWork.DoWork();
            }
        }

        private void Reset()
        {
            _currentDoingWork = null;
            _sumProcedurePoint = 0;
            _completedProcedurePoint = 0;
            _works.Clear();
        }

        public ProcedureWorkerStation()
        {
            _works = new Queue<ProcedureWork>();
            IsWorking = false;
        }

        private Queue<ProcedureWork> _works;
        private ProcedureWork _currentDoingWork;
        private int _sumProcedurePoint;
        private int _completedProcedurePoint;
    }

    public enum AddProcedureType
    {
        Parallel,
        Serial,
    }
}