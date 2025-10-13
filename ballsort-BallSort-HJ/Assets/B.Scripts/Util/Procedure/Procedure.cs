using System;
using UnityEngine;

namespace SoyBean.Procedure
{
    public class Procedure
    {
        public Action<Procedure> OnCompleted = delegate { };
        
        public Action Work;
        public bool IsCompleted;
        public bool IsWorking;
        public int Point;

        public Procedure(int Point)
        {
            this.Point = Point;
            IsCompleted = false;
            IsWorking = false;
        }

        public void Completed()
        {
            IsCompleted = true;
            IsWorking = false;
            OnCompleted.Invoke(this);
        }

        public void StartWork()
        {
            if (!IsCompleted)
            {
                IsWorking = true;
                Work.Invoke();
            }
            else
            {
                Debug.LogError("Start a finished work.");
            }
        }
    }
}