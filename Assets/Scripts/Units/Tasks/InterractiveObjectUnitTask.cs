using UnityEngine;
using System.Collections;

public class InterractiveObjectUnitTask : IUnitTask
{
    public WorkerUnit unit { get; set; }
    public InterractiveObject currentObject { get; set; }

    public IEnumerator BeginTask()
    {
        currentObject.BeginWork(unit);

        while (currentObject != null && currentObject.Progress < 1.0F)
        {
            yield return new WaitForSeconds(0.25F);
        }
    }

    public void AbordTask()
    {
        currentObject?.EndWork(unit);
    }
}