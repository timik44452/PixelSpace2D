using UnityEngine;

using Game;
using Game.Unit;


[RequireComponent(typeof(PathFinder))]
public class WorkerUnit : MonoBehaviour, ISelectable, IStrategicUnit
{
    private enum State
    { 
        Normal,
        FiningPlaceToStay,
        Resting
    }

    public float speed = 1.0F;

    public float health
    {
        get => _health;
        set => _health = Mathf.Clamp(value, 0, 100);
    }
    public float hunger
    {
        get => _hunger;
        set => _hunger = Mathf.Clamp(value, 0, 100);
    }
    public float tiredness
    {
        get => _tiredness;
        set => _tiredness = Mathf.Clamp(value, 0, 100);
    }

    public Ship currentShip
    {
        get;
        private set;
    }
    public PathFinder currentPathFinder
    {
        get;
        private set;
    }

    private float _health = 100;
    private float _hunger = 0;
    private float _tiredness = 0;

    private State _state = State.Normal;

    private UnitTaskManager _taskManager;

    private void Start()
    {
        currentShip = GetComponentInParent<Ship>();
        currentPathFinder = GetComponent<PathFinder>();
        _taskManager = new UnitTaskManager(this);
    }

    private void FixedUpdate()
    {
        if (tiredness > 15F && _state == State.Normal)
        {
            _state = State.FiningPlaceToStay;
        }

        if (_tiredness < 5F && _state != State.Normal)
        {
            _state = State.Normal;
        }

        if (_state == State.FiningPlaceToStay)
        {
            foreach (Bed bed in currentShip.GetInterractiveObjects<Bed>())
            {
                if (bed.IsProcessed() == false)
                {
                    _taskManager.RegisterPriorityTasks(
                        UnitTaskFactory.Create(this, bed.transform.localPosition),
                        UnitTaskFactory.Create(this, bed));

                    _state = State.Resting;
                }
            }
        }

        tiredness += Time.fixedDeltaTime;
        hunger += Time.fixedDeltaTime;
    }

    public void OnInvoke(Vector2 point, GameObject target)
    {
        Vector2 localPoint = currentShip.transform.worldToLocalMatrix.MultiplyPoint(point);
        
        _taskManager.RegisterTasks(
            UnitTaskFactory.Create(this, localPoint),
            UnitTaskFactory.Create(this, target.GetComponent<InterractiveObject>()));
    }

    public void OnSelected()
    {
    }

    public void OnDeselected()
    {
    }

}
