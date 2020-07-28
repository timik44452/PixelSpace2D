using UnityEngine;

public class EngineControlTerminal : Terminal
{
    public Vector3 direction = Vector3.zero;
    public bool IsHandWorkMode = false;

    private Ship m_ship;

    private const float threshould = 0.01F;

    private void Start()
    {
        m_ship = GetComponentInParent<Ship>();
    }

    private void FixedUpdate()
    {
        Vector2 inputDirection = GetInputVector();

        Vector2 _velocityOffset = inputDirection.y * transform.forward;
        float _angularVelocityOffset = inputDirection.x * 30;

        Vector2 velocity = m_ship.currentRigidbody2D.velocity + _velocityOffset;
        float angularVelocity = m_ship.currentRigidbody2D.angularVelocity + _angularVelocityOffset;

        foreach (Engine engine in m_ship.GetComponentsInChildren<Engine>())
        {
            Vector2 up = transform.worldToLocalMatrix.MultiplyVector(engine.transform.up);
            Vector2 comDirection = (m_ship.currentRigidbody2D.centerOfMass - (Vector2)engine.transform.localPosition).normalized;

            float force = inputDirection.y;
            float angular = GetAngular(up, comDirection);
            
            if (Mathf.Abs(angular) > threshould && Mathf.Abs(angularVelocity) > threshould)
            {
                force = Mathf.Clamp(-angularVelocity / angular, 0, 1);
            }

            engine.Thrust(force);
        }

        direction *= 0.5F;
    }

    private Vector2 GetInputVector()
    {
        if (IsHandWorkMode)
        {
            return new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));
        }

        return direction;
    }

    private float GetAngular(Vector2 forceDirection, Vector2 delta)
    {
        if (forceDirection.magnitude == 0 || delta.magnitude == 0)
        {
            return 0;
        }

        float cosa = (forceDirection.x * delta.x + forceDirection.y * delta.y) / (forceDirection.magnitude * delta.magnitude);
        float sina = Mathf.Sqrt(1F - cosa);
        float sign = (forceDirection.x * delta.y - forceDirection.y * delta.x);

        return sina * sign;
    }
}
