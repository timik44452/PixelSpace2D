using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveUnitTask : IUnitTask
{
    public WorkerUnit unit { get; set; }
    public Vector2 targetPoint;

    public IEnumerator BeginTask()
    {
        List<Vector2Int> path = unit.currentPathFinder.FindPath(unit.transform.localPosition, targetPoint, unit.currentShip.currentData);

        if (path.Count == 0)
        {
            yield break;
        }

        for (int pointIndex = 0; pointIndex < path.Count; pointIndex++)
        {
            float step = unit.speed * Time.deltaTime;

            Vector2 currentPoint = path[pointIndex];

            LookAt(currentPoint);

            for (float alpha = 0; alpha < 1.0F + step; alpha += step)
            {
                Vector2 newPoint = Vector2.Lerp(unit.transform.localPosition, currentPoint, alpha);

                unit.transform.localPosition = new Vector3(newPoint.x, newPoint.y, unit.transform.localPosition.z);

                yield return null;
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
