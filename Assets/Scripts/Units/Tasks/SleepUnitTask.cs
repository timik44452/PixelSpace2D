
using System.Collections;
using UnityEngine;

public class SleepUnitTask : IUnitTask
{
    public WorkerUnit unit { get; set; }
    private Bed currentBed = null;

    public IEnumerator BeginTask()
    {
        while (currentBed == null)
        {
            foreach (var bed in unit.currentShip.GetComponentsInChildren<Bed>())
            {
                if (bed.currentUnit == null)
                {
                    bed.currentUnit = unit;
                    currentBed = bed;
                }
            }

            yield return new WaitForSeconds(0.25F);
        }

        while (unit.tiredness < 100)
        {
            unit.tiredness -= (Time.fixedDeltaTime + Time.deltaTime) * 5;

            yield return null;
        }

        Debug.Log("End sleep");
    }

    public void AbordTask()
    {
        currentBed.currentUnit = null;
    }
}