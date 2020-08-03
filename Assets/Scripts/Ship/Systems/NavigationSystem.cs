using System;
using UnityEngine;

public class NavigationSystem : MonoBehaviour
{
    private EngineControlTerminal engineControlTerminal;

    private void FixedUpdate()
    {
        if(engineControlTerminal == null)
        {
            engineControlTerminal = FindObjectOfType<EngineControlTerminal>();
            return;
        }

        Vector2 direction = (Vector3.one * 100 - transform.position).normalized;

        float rotation = 0;// GetRotation(transform.up, direction);

        engineControlTerminal.direction = new Vector3(rotation, 0, 0);
    }

    private float GetRotation(Vector2 fromDirection, Vector2 toDirection)
    {
        float sign = Math.Sign(fromDirection.x * toDirection.y - fromDirection.y * toDirection.x);
        return sign * Vector2.Angle(fromDirection, toDirection) / 180F;
    }
}
