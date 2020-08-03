public abstract class SingleunitInterractiveObject : InterractiveObject
{
    public WorkerUnit currentUnit;

    public virtual bool TryChangeUnit(WorkerUnit unit)
    {
        return false;
    }

    public override bool TryBeginWork(WorkerUnit unit)
    {
        if (currentUnit == null || TryChangeUnit(unit))
        {
            currentUnit = unit;

            return true;
        }

        return false;
    }

    public override void EndWork(WorkerUnit unit)
    {
        if (currentUnit == unit)
        {
            currentUnit = null;
        }
    }
}
