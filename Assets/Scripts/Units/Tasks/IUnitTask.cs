using System.Collections;

public interface IUnitTask
{
    WorkerUnit unit { get; set; }

    IEnumerator BeginTask();
    void AbordTask();
}