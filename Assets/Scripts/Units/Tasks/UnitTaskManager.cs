using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public class UnitTaskManager
    {
        private WorkerUnit unit;
        private IUnitTask currentTask = null;
        private Coroutine taskCoroutine = null;
        private List<IUnitTask> unitTasks = new List<IUnitTask>();

        public UnitTaskManager(WorkerUnit unit)
        {
            this.unit = unit;
            this.unit.StartCoroutine(CoreThread());
        }

        public void RegisterTasks(params IUnitTask[] tasks)
        {
            unitTasks.AddRange(tasks);
        }

        public void RegisterPriorityTasks(params IUnitTask[] tasks)
        {
            AbordTask();
            unitTasks.InsertRange(0, tasks);
        }

        public void AbordTask()
        {
            if (currentTask == null)
            {
                return;
            }

            currentTask.AbordTask();
            unit.StopCoroutine(taskCoroutine);
            currentTask = null;
        }

        private IEnumerator CoreThread()
        {
            while (true)
            {
                if (currentTask == null && unitTasks.Count > 0)
                {
                    InvokeFirstTask();
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator TaskHandler(IUnitTask task)
        {
            yield return task?.BeginTask();

            taskCoroutine = null;
            currentTask = null;
        }

        private void InvokeFirstTask()
        {
            if (unitTasks.Count > 0)
            {
                IUnitTask unitTask = unitTasks[0];
                unitTasks.RemoveAt(0);

                InvokeTask(unitTask);
            }
        }

        private void InvokeTask(IUnitTask task)
        {
            currentTask = task;
            taskCoroutine = unit.StartCoroutine(TaskHandler(task));
        }
    }
}
