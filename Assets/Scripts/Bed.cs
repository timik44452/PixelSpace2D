using Game;
using UnityEngine;

public class Bed : SingleunitInterractiveObject, ISelectable
{
    public override bool IsProcessed()
    {
        return currentUnit != null && currentUnit.tiredness > 0;
    }

    public void OnDeselected()
    {
    }

    public void OnSelected()
    {
    }

    private void Update()
    {
        if (IsProcessed())
        {
            currentUnit.tiredness -= (Time.fixedDeltaTime + Time.deltaTime) * 2;
        }
    }
}
