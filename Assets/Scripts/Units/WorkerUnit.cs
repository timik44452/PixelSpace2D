using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

[RequireComponent(typeof(PathFinder))]
public class WorkerUnit : MonoBehaviour, ISelectable, IStrategicUnit
{
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

    private IUnitTask currentTask = null;
    private Coroutine taskCoroutine = null;
    private List<IUnitTask> unitTasks = new List<IUnitTask>();


    private void Start()
    {
        currentShip = GetComponentInParent<Ship>();
        currentPathFinder = GetComponent<PathFinder>();
    }

    private void FixedUpdate()
    {
        Debug.Log(tiredness);
        if (tiredness > 65F)
        {
            AbordTask();
            InvokeTask(UnitTaskFactory.Create<SleepUnitTask>(this));
        }

        if (currentTask == null && unitTasks.Count > 0)
        {
            InvokeTask(GetAndRemoveTask());
        }

        tiredness += Time.fixedDeltaTime;
        hunger += Time.fixedDeltaTime;
    }

    public void OnInvoke(Vector2 point, GameObject target)
    {
        Vector2 localPoint = currentShip.transform.worldToLocalMatrix.MultiplyPoint(point);
        
        RegisterTask(UnitTaskFactory.Create(this, localPoint));
        RegisterTask(UnitTaskFactory.Create(this, target.GetComponent<InterractiveObject>()));
    }

    public void OnSelected()
    {
    }

    public void OnDeselected()
    {
    }

    private void RegisterTask(IUnitTask task, bool isPriority = false)
    {
        if (task != null)
        {
            if (isPriority)
            {
                AbordTask();
                unitTasks.Insert(0, task);
            }
            else
            {
                unitTasks.Add(task);
            }
        }
    }

    private void AbordTask()
    {
        if (currentTask == null)
        {
            return;
        }

        currentTask.AbordTask();
        StopCoroutine(taskCoroutine);
        currentTask = null;
    }

    private void InvokeTask(IUnitTask task)
    {
        currentTask = task;
        taskCoroutine = StartCoroutine(TaskHandler(task));
    }

    private IEnumerator TaskHandler(IUnitTask task)
    {
        yield return task?.BeginTask();

        taskCoroutine = null;
        currentTask = null;
    }

    private IUnitTask GetAndRemoveTask()
    {
        if (unitTasks.Count > 0)
        {
            IUnitTask unitTask = unitTasks[0];
            unitTasks.RemoveAt(0);
            return unitTask;
        }
        else
        {
            return null;
        }
    }
}
