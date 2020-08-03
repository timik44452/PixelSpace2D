using UnityEngine;
using System.Collections;

public class InterractiveObjectUnitTask : IUnitTask
{
    public WorkerUnit unit { get; set; }
    public InterractiveObject currentObject { get; set; }

    public IEnumerator BeginTask()
    {
        if (!currentObject.TryBeginWork(unit))
        {
            yield break;
        }

        while (currentObject != null && currentObject.IsProcessed())
        {
            yield return new WaitForSeconds(0.25F);
        }

        AbordTask();
    }

    public void AbordTask()
    {
        currentObject?.EndWork(unit);
    }
}