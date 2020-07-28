using UnityEngine;

public class Engine : MonoBehaviour
{
    public float force = 0.0F;
    public float temperature = 0.0F;

    public bool autoDisable = false;
    public bool forcedMode = false;

    private Rigidbody2D m_rigidbody2D;
    private ParticleSystem m_particleSystem;

    private bool _enabled = false;
    private bool _overheatAutoDisable = false;

    private const float forceModeMultiply = 5F;
    private const float disableOverheatTemperature = 750F;
    private const float overheatTemperature = 1000F;


    public void Thrust(float power)
    {
        force = Mathf.Clamp01(power);

        if (forcedMode)
        {
            force *= forceModeMultiply;
        }

        if (force > 0)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }

    public void Enable()
    {
        _enabled = true;
        CheckAuthoDisable();
        UpdateParticleState();
    }

    public void Disable()
    {
        _enabled = false;
        UpdateParticleState();
    }

    private void Awake()
    {
        m_particleSystem = GetComponentInChildren<ParticleSystem>();

        Disable();
    }

    private void FixedUpdate()
    {
        if(m_rigidbody2D == null)
        {
            m_rigidbody2D = GetComponentInParent<Rigidbody2D>();
            return;
        }

        temperature = Mathf.Clamp(temperature + (force - 1) * 20 * Time.fixedDeltaTime, 0, overheatTemperature * 2);

        if (_overheatAutoDisable)
        {
            _overheatAutoDisable = temperature > (disableOverheatTemperature - (overheatTemperature - disableOverheatTemperature));
        }
        else if (_enabled)
        {
            m_rigidbody2D.AddForceAtPosition(transform.up * force, transform.position);
        }
    }

    private void CheckAuthoDisable()
    {
        if (autoDisable && temperature > disableOverheatTemperature)
        {
            _overheatAutoDisable = true;
        }

        if (_overheatAutoDisable)
        {
            force = 0;
            _enabled = false;
        }
    }

    private void UpdateParticleState()
    {
        m_particleSystem.gameObject.SetActive(_enabled);
        var module = m_particleSystem.main;

        module.startSpeedMultiplier = force;
    }
}
