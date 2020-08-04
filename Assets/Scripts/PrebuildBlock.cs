using Game;
using UnityEngine;

public class PrebuildBlock : MultiunitInterractiveObject, ISelectable
{
    public int blockID = 0;
    public float rotation = 0;
    public Ship currentShip;

    private float progress = 0.0F;
    private float endProgress = 1F;
    

    private void FixedUpdate()
    {
        if(currentShip == null)
        {
            Destroy(gameObject);
        }

        progress += involvedUnits.Count * Time.fixedDeltaTime;

        if (progress >= endProgress)
        {
            int x = Mathf.RoundToInt(transform.localPosition.x);
            int y = Mathf.RoundToInt(transform.localPosition.y);

            ShipBlock block = ShipBlock.Create(blockID, x, y, rotation);

            currentShip.currentData.AddBlock(block);

            currentShip.BuildShip();

            Destroy(gameObject);
        }
    }

    public override bool IsProcessed()
    {
        return progress < endProgress;
    }

    public void OnSelected()
    {
    }

    public void OnDeselected()
    {
    }
}
