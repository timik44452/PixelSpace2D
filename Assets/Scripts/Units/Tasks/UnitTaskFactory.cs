using UnityEngine;

public class UnitTaskFactory
{
    public static IUnitTask Create(WorkerUnit unit, Vector2 targetPoint)
    {
        MoveUnitTask unitTask = Create<MoveUnitTask>(unit);

        unitTask.targetPoint = targetPoint;

        return unitTask;
    }

    public static IUnitTask Create(WorkerUnit unit, InterractiveObject interractiveObject)
    {
        if (interractiveObject == null)
        {
            return null;
        }

        InterractiveObjectUnitTask unitTask = Create<InterractiveObjectUnitTask>(unit);

        unitTask.currentObject = interractiveObject;

        return unitTask;
    }

    public static T Create<T>(WorkerUnit unit) where T : IUnitTask, new()
    {
        T task = new T();

        task.unit = unit;

        return task;
    }
}