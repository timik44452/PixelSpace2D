using Game.ShipService;
using System;
using UnityEngine;

public class EngineControlTerminal : MonoBehaviour
{
    [System.Serializable]
    public class PIDOptions
    {
        public float P = 0.7F;
        public float I = 0.01F;
        public float D = 0.2F;
    }

    public Vector3 direction = Vector3.zero;
    public PIDOptions options = new PIDOptions();

    public bool IsHandWorkMode = false;
    public bool EngineAutoDisable = false;
    public bool EngineForceMode = false;
    public bool LimitAngularVelocity = true;

    private Ship _ship;
    private PIDController _angularPID;

    private const float maxAngularVelocity = 1000;
    private const float limitedAngularVelocity = 50;
    private const float threshould = 0.01F;

    
    private void Start()
    {
        _ship = GetComponentInParent<Ship>();
        _angularPID = new PIDController(options.P, options.I, options.D);
    }

    private void FixedUpdate()
    {
        Vector2 inputDirection = GetInputVector();

        float angularVelocityOffset = inputDirection.x * (LimitAngularVelocity ? limitedAngularVelocity : maxAngularVelocity);
        float angularVelocity = _ship.currentRigidbody2D.angularVelocity + angularVelocityOffset;

        _angularPID.P = options.P;
        _angularPID.I = options.I;
        _angularPID.D = options.D;

        foreach (Engine engine in _ship.GetComponentsInChildren<Engine>())
        {
            Vector2 up = transform.worldToLocalMatrix.MultiplyVector(engine.transform.up);
            Vector2 comDirection = (_ship.currentRigidbody2D.centerOfMass - (Vector2)engine.transform.localPosition).normalized;

            float angular = GetAngular(up, comDirection);
            float force = 0;

            if (Math.Abs(angularVelocity) > threshould)
            {
                force = _angularPID.PID(angularVelocity / angular, 0);
            }

            if (Mathf.Abs(force) < threshould)
            {
                force = 0;
            }

            if(Mathf.Sign(inputDirection.y) == Mathf.Sign(Vector2.Dot(_ship.transform.up, engine.transform.up)))
            {
                force += Mathf.Abs(inputDirection.y);
            }

            engine.Thrust(force, GetForcing());
        }
    }

    private float GetForcing()
    {
        return EngineForceMode && (!IsHandWorkMode || Input.GetKey(KeyCode.LeftShift)) ? 1 : 0;
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

    private GUIGraph graph = new GUIGraph();

    private void OnGUI()
    {
        graph.Draw(new Rect(0, 0, 250, 250));
    }
}
