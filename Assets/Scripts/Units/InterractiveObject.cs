using UnityEngine;

public abstract class InterractiveObject : MonoBehaviour
{
    public abstract bool IsProcessed();

    public abstract bool TryBeginWork(WorkerUnit unit);

    public abstract void EndWork(WorkerUnit unit);
}