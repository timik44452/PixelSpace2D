using System.Collections.Generic;

public abstract class MultiunitInterractiveObject : InterractiveObject
{
    public List<WorkerUnit> involvedUnits { get; private set; } = new List<WorkerUnit>();

    // unlimited units on interrractive object by default
    public virtual bool InvolvedUnitCountIsValid(int count)
    {
        return true;
    }

    public override bool TryBeginWork(WorkerUnit unit)
    {
        if (InvolvedUnitCountIsValid(involvedUnits.Count))
        {
            if (!involvedUnits.Contains(unit))
            {
                involvedUnits.Add(unit);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public override void EndWork(WorkerUnit unit)
    {
        involvedUnits.Remove(unit);
    }
}
