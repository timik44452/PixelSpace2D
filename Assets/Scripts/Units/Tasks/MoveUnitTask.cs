using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveUnitTask : IUnitTask
{
    public WorkerUnit unit { get; set; }
    public Vector2 targetPoint;

    public IEnumerator BeginTask()
    {
        float z = unit.transform.localPosition.z;
        List<Vector2Int> path = unit.currentPathFinder.FindPath(unit.transform.localPosition, targetPoint, unit.currentShip.currentData);

        if (path.Count == 0)
        {
            yield break;
        }

        for (int pointIndex = 0; pointIndex < path.Count; pointIndex++)
        {
            Vector2 currentPoint = path[pointIndex];

            LookAt(currentPoint);

            float step = 0F;

            while (Vector2.Distance(unit.transform.localPosition, currentPoint) > step * 1.5F)
            {
                step = unit.speed * Time.deltaTime;
                Vector3 direction = (currentPoint - (Vector2)unit.transform.localPosition).normalized;
                Vector3 position = unit.transform.localPosition + direction * step;
                position.z = z;

                unit.transform.localPosition = position;

                yield return new WaitForEndOfFrame();
            }
        }

        LookAt(targetPoint);
    }

    public void AbordTask()
    {
        
    }

    private void LookAt(Vector2 point)
    {
        Vector2 direction = point - (Vector2)unit.transform.localPosition;

        unit.transform.localRotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, direction) * Mathf.Sign(-direction.x));
    }
}
