using UnityEngine;
using System.Collections.Generic;

public abstract class InterractiveObject : MonoBehaviour
{
    public float Progress
    {
        get => _progress;
        protected set => _progress = Mathf.Clamp01(value);
    }
    public List<WorkerUnit> involvedUnits { get; private set; } = new List<WorkerUnit>();


    private float _progress = 0;


    public void BeginWork(WorkerUnit unit)
    {
        if (!involvedUnits.Contains(unit))
        {
            involvedUnits.Add(unit);
        }
    }

    public void EndWork(WorkerUnit unit)
    {
        involvedUnits.Remove(unit);
    }
}
